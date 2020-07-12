var BUS_DATA = null;
var ERROR_MESSAGE = "Unable to Connect";
var LOG = null;
var NOTIFICATIONS = null;
var STATUS = null;
var STATUS_TIMEOUT = null;
var PORT = 9287;

const cameraStatus = ['Unknown', 'Recording', 'Idle', 'Initialising'];
const cameraStatusBG = ['secondary', 'danger', 'info', 'dark'];
const notificationStatusBG = ['', 'success', 'warning', 'danger'];

const displayWindows = ['.bigCamera', '.cameras', '.dataTables_length', '.log', '.mainDashboard'];

$(document).ready(function () {
    SendCommand({ CommandName: 'initial_load', "Token": AUTH_KEY });
    UpdateTime();
});

function BigCamera(camera, state, source) {
    $('.bigCamera').html(`<div class="card"><div class="card-header text-white bg-${cameraStatusBG[state]}">${camera} (${cameraStatus[state]})</div>
    <div class="card-body"><video width="100%" height="762px" controls autoplay muted><source src="${source}" type="video/mp4">Your browser does not support the video tag.</video></div></div>`);

    Display(displayWindows, '.bigCamera');
}

function Display(hide, show) {
    for (var i = 0; i < hide.length; i++)
        $(hide[i]).hide();

    $(show).show();
}

function LoadDoors() {
    $(".controls tr").remove();

    BUS_DATA.Doors.forEach(door => {

        const Status = (door.State == 1)
            ? `<td><a href="#" onclick="ToggleDoor('${door.Name}')" id="${door.Name}-door" class="btn btn-large btn-tall btn-block btn-danger">Close</a></td>`
            : `<td><a href="#" onclick="ToggleDoor('${door.Name}')" id="${door.Name}-door" class="btn btn-large btn-tall btn-block btn-success">Open</a></td>`;

        if (door.RemoteOperation)
            $('.controls').append(`<tr><td style="width:30%">${door.Name}</td><td>${Status}</td></tr>`);
    });
}

function LoadLog() {
    SendCommand({ CommandName: 'get_command_log', Logged: false, "Token": AUTH_KEY });
}

async function LoadUI() {
    const cameraStorageBG = ['success', 'warning', 'danger', 'info'];
    const windowStates = ['Unknown', 'Open', 'Closed'];

    $(".controls tr").remove();
    $(".windows tr").remove();

    var cameraIcons = ``;
    var cameraStorage = ``;
    var cameraStorageFree = 0;

    for (var i = 0; i < BUS_DATA.CCTVDrives.length; i++) {

        if (!BUS_DATA.CCTVDrives[i].Monitor)
            continue;


        if (BUS_DATA.CCTVDrives[i].Used >= 0 && BUS_DATA.CCTVDrives[i].Used < 50)
            cameraStorageFree = cameraStorageBG[0];

        if (BUS_DATA.CCTVDrives[i].Used >= 50 && BUS_DATA.CCTVDrives[i].Used < 90)
            cameraStorageFree = cameraStorageBG[1];

        if (BUS_DATA.CCTVDrives[i].Used >= 90)
            cameraStorageFree = cameraStorageBG[2];

        cameraStorage += `<div class="col-sm-5"><pre class="row h-100 justify-content-left align-items-center pl-3">${BUS_DATA.CCTVDrives[i].DriveLetter}${BUS_DATA.CCTVDrives[i].Folder} (${BUS_DATA.CCTVDrives[i].Used}%)</pre></div>
        <div class="col-sm-7"><p><div class="progress">
        <div class="progress-bar bg-${cameraStorageFree}" role="progressbar" style="width: ${BUS_DATA.CCTVDrives[i].Used}%" aria-valuenow="${BUS_DATA.CCTVDrives[i].Used}" aria-valuemin="0" aria-valuemax="100"></div></div></p></div>`;
    }
    $('.cameraStorage').html(cameraStorage);

    // Cameras
    var cameraHTML = `<div class="row pb-2">`;
    for (var i = 0; i < BUS_DATA.Cameras.length; i++) {
        var video;

        cameraIcons += `<div class="col-sm"><a href="#" onclick="BigCamera('${BUS_DATA.Cameras[i].Name}', '${BUS_DATA.Cameras[i].State}', 'rtsp://${BUS_DATA.Cameras[i].IPAddress}:${BUS_DATA.Cameras[i].Port}')"><i title="${BUS_DATA.Cameras[i].Name} (${cameraStatus[BUS_DATA.Cameras[i].State]})" class="text-center fas fa-video text-${cameraStatusBG[BUS_DATA.Cameras[i].State]} h3"></i></a></div>`

        switch (BUS_DATA.Cameras[i].State) {
            case 0:
                video = `<div class="row h-100 justify-content-center align-items-center">${cameraStatus[BUS_DATA.Cameras[i].State]} feed status</div>`;
                break;

            case 1:
                video = `<video width="100%" height="220px" controls><source id="rtsp://${BUS_DATA.Cameras[i].IPAddress}:${BUS_DATA.Cameras[i].Port}" type="video/mp4">Your browser does not support the video tag.</video>`;
                break;

            case 2:
                video = `<div class="row h-100 justify-content-center align-items-center">Camera is ${cameraStatus[BUS_DATA.Cameras[i].State]}</div>`;
                break;

            case 3:
                video = `<div class="row h-100 justify-content-center align-items-center"><div class="spinner-grow text-${cameraStatusBG[BUS_DATA.Cameras[i].State]}"></div></div>`;
                break;
        }

        if (i % 2 === 0 && i !== 0) {
            cameraHTML += `</div></div><div class="row pb-2 pt-2">`;
        }

        cameraHTML += `<div class="col-md-6"><div class="card" style="height:315px;">
            <div class="card-header text-white bg-${cameraStatusBG[BUS_DATA.Cameras[i].State]}">${BUS_DATA.Cameras[i].Name} (${cameraStatus[BUS_DATA.Cameras[i].State]})</div>
            <div class="card-body">${video}</div></div></div>`;

    }
    $('.cameras').html(cameraHTML + "</div>");
    $('.cameraIcons').html(cameraIcons);

    //Windows
    var windowData = ``;
    for (var i = 0; i < BUS_DATA.Windows.length; i++) {
        windowData += `<div class="col-sm-7"><p class="h-100 text-center align-items-center pl-3">${BUS_DATA.Windows[i].FriendlyName}</p></div>
                    <div class="col-sm-5"><p class="h-100 text-center align-items-center pl-3">${windowStates[BUS_DATA.Windows[i].State]}</p></div> `;
    }
    $('.windows').html(windowData);
    Display(['.loader'], '.wrapper');

}

function LoadStatus(){
    SendCommand({ CommandName: "get_notifications", Logged: false, "Token": AUTH_KEY });
}

function NotificationRead(id){
    SendCommand({ CommandName: 'mark_read', CommandParamaters: [id], "Token": AUTH_KEY });
}

function Reload() {
    Display(displayWindows, '.cameras');

    SendCommand({ CommandName: 'reload_config', "Token": AUTH_KEY });
}

function SendCommand(command) {
    var commandSocket = new WebSocket(`wss://localhost:${PORT}`);

    commandSocket.onopen = function () {
        commandSocket.send(JSON.stringify(command));
    };

    commandSocket.onmessage = function (event) {
        var response = JSON.parse(event.data);

        if (response['error_message'] !== undefined) {
            ERROR_MESSAGE = response.errorMessage;
            $('.invalid').text(ERROR_MESSAGE);
            Display(displayWindows, '.invalid');
            return;
        }

        commandSocket.close();
        SendCommand_callback(response);
    };

    commandSocket.onerror = function () {
        commandSocket.close();
    };
}

async function SendCommand_callback(data) {

    console.table(data);

    var response = { Command: data.Command, Data: data.Data };

    switch (response.Command) {

        case "get_command_log":
            LOG = response.Data;
            ShowLog();
            return;

        case "get_notifications":
            NOTIFICATIONS = response.Data;
            UpdateStatus();
            return;

        case "get_status":
            STATUS = response.Data;
            UpdateStatus();
            return;

        case "initial_load":
            BUS_DATA = response.Data;
            await LoadUI().then(function () {
                LoadDoors();
                LoadStatus();
            });
            return;

        case "reload_config":
            BUS_DATA = response.Data;
            await LoadUI().then(function () {
                LoadDoors();
                LoadStatus();
            });
            return;

        case "toggle_door":
            BUS_DATA.Doors = response.Data;
            LoadDoors();
            return;

    }

}

function ShowLog() {

    $('.log').html(`<div class="card" style="height: 975px"><div class="card-header text-white bg-info">Log</div>
    <div class="card-body"><table class="logEntries noselect"><thead><tr><th>Name</th><th>Params</th><th>Token</th></tr></thead><tbody><tr></tr></tbody></table></div></div>`);

    $(".logEntries tbody tr").remove();
    LOG.forEach(logEntry => {
        $('.logEntries').append(`<tr><td>${logEntry.CommandName}</td><td>${JSON.stringify(logEntry.CommandParamaters)}</td><td>${logEntry.Token}</td></tr>`);
    });

    $('.logEntries').DataTable({ searching: false, iDisplayLength: 19 });

    Display(displayWindows, '.log');
}

function ToggleDoor(name) {
    SendCommand({ "CommandName": "toggle_door", "CommandParamaters": [name], "Token": AUTH_KEY });
}

function ToggleMenu(){


    Display(displayWindows, '.mainDashboard');
}

function UpdateStatus(){
    $('.systemStatus').html(STATUS);
    $(".notificationsTable tr").remove();

    NOTIFICATIONS.forEach(notification => {
        $('.notificationsTable').append(`<tr><td align="left" style="width: 20%;" class="${notificationStatusBG[notification.Priority]}">${notification.Name}</td><td align="left">${notification.Contents}</td><td align="right" style="width:5%;"><a href="#" onclick="NotificationRead(${notification.Id});"><i class="fas fa-trash text-dark h5"></i></a></td></tr>`);
    });

    let unread = 0;
    NOTIFICATIONS.forEach(notification => {
        if(notification.Read === false){
            unread++;
        }
    });

    if(unread > 0)
        $('.systemStatus').html(`<b>Hey</b>, You have ${unread} unread notification(s)!`);
    else
        $('.systemStatus').html(`<span class="text-success">The system is running perfectly!</span>`);
}

function UpdateTime() {
    var today = new Date(), h = today.getHours(), m = today.getMinutes(), s = today.getSeconds();

    h = (h < 10) ? `0${h}` : h;
    m = (m < 10) ? `0${m}` : m;
    s = (s < 10) ? `0${s}` : s;

    $('.time').html(`${h}:${m}:${s}  <i class="pl-2 fas fa-clock"></i>`);

    t = setTimeout(function () { UpdateTime() }, 500);
}
