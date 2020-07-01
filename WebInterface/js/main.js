var BUS_DATA = null;
var ERROR_MESSAGE = "Unable to Connect";
var LOG = null;
var PORT = 9287;

const cameraStatus = ['Unknown', 'Recording', 'Idle', 'Initialising'];
const cameraStatusBG = ['secondary', 'danger', 'info', 'dark'];

$(document).ready(function () {
    SendCommand({ CommandName: 'initial_load', "Token": AUTH_KEY });
    UpdateTime();
});

function BigCamera(camera, state, source) {
    $('.bigCamera').html(`<div class="card"><div class="card-header text-white bg-${cameraStatusBG[state]}">${camera} (${cameraStatus[state]})</div>
    <div class="card-body"><video width="100%" height="762px" controls autoplay muted><source src="${source}" type="video/mp4">Your browser does not support the video tag.</video></div></div>`);

    Display(['.log', '.cameras'], '.bigCamera');
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

    // Camera Controls
    var cameraIcons = ``;
    var cameraStorage = ``;
    for (var i = 0; i < BUS_DATA.CCTVPaths.length; i++) {

        if (!BUS_DATA.CCTVPaths[i].Monitor)
            continue;

        var cameraStorageFree = 0;

        if (BUS_DATA.CCTVPaths[i].Used >= 0 && BUS_DATA.CCTVPaths[i].Used < 33)
            cameraStorageFree = cameraStorageBG[0];

        if (BUS_DATA.CCTVPaths[i].Used >= 33 && BUS_DATA.CCTVPaths[i].Used < 66)
            cameraStorageFree = cameraStorageBG[1];

        if (BUS_DATA.CCTVPaths[i].Used >= 66)
            cameraStorageFree = cameraStorageBG[2];


        cameraStorage += `<div class="col-sm-5"><pre class="row h-100 justify-content-left align-items-center pl-3">${BUS_DATA.CCTVPaths[i].Drive}${BUS_DATA.CCTVPaths[i].Folder} (${BUS_DATA.CCTVPaths[i].Used}%)</pre></div>
        <div class="col-sm-7"><p><div class="progress">
        <div class="progress-bar bg-${cameraStorageFree}" role="progressbar" style="width: ${BUS_DATA.CCTVPaths[i].Used}%" aria-valuenow="${BUS_DATA.CCTVPaths[i].Used}" aria-valuemin="0" aria-valuemax="100"></div></div></p></div>`;
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
    Display(['.invalid', '.loader'], '.wrapper');
}

function Reload() {
    Display(['.bigCamera', '.log'], '.cameras');
    SendCommand({ CommandName: 'reload_config', "Token": AUTH_KEY });
}

async function SendCommand(command) {
    var commandSocket = new WebSocket(`wss://localhost:${PORT}`);

    commandSocket.onopen = function () {
        commandSocket.send(JSON.stringify(command));
    };

    commandSocket.onmessage = function (event) {
        var response = JSON.parse(event.data);

        if (response['errorMessage'] !== undefined) {
            ERROR_MESSAGE = response.errorMessage;
            $('.invalid').text(ERROR_MESSAGE);
            Display(['.wrapper', '.loader'], '.invalid');
            return;
        }

        SendCommand_callback(response);
    };

    commandSocket.onerror = function () {
        $('.invalid').text(ERROR_MESSAGE);
        Display(['.wrapper'], '.invalid');
        setTimeout(() => { window.location.reload(); }, 1000);
    };
}

async function SendCommand_callback(data) {

    var response = { Callback: data.Callback, Data: data.Data };

    switch (response.Callback) {

        case "get_command_log":
            LOG = response.Data;
            ShowLog();
            return;

        case "initial_load":
            BUS_DATA = response.Data;
            await LoadUI().then(function () {
                LoadDoors();
            });
            return;

        case "reload_config":
            BUS_DATA = response.Data;
            await LoadUI().then(function () {
                LoadDoors();
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

    Display(['.bigCamera', '.cameras', '.dataTables_length'], '.log');
}

function ToggleDoor(name) {
    SendCommand({ "CommandName": "toggle_door", "CommandParamaters": [name], "Token": AUTH_KEY });
}

function UpdateTime() {
    var today = new Date(),
        h = today.getHours(),
        m = today.getMinutes(),
        s = today.getSeconds();

    h = (h < 10) ? `0${h}` : h;
    m = (m < 10) ? `0${m}` : m;
    s = (s < 10) ? `0${s}` : s;

    var time = `${h}:${m}:${s}`;

    $('.time').html(`${time}  <i class="fas fa-clock"></i>`);

    t = setTimeout(function () {
        UpdateTime()
    }, 500);
}
