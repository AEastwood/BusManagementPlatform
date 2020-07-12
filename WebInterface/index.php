<?php
    $AuthKey = file_get_contents('./config/bus.secret');
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <link type="text/css" href="bootstrap/css/bootstrap.min.css" rel="stylesheet">
        <link type="text/css" href="css/jquery.dataTables.min.css" rel="stylesheet">
        <link type="text/css" href="css/theme.css" rel="stylesheet">
        <link type="text/css" href="images/icons/css/font-awesome.css" rel="stylesheet">
        <link type="text/css" href='css/fonts.css' rel='stylesheet'>
        <script src="js/8bf1f276fe.js" crossorigin="anonymous"></script>
        <script src="jquery/jquery.min.js"></script>
        <script src="js/main.js" crossorigin="anonymous"></script>
        <?php echo "<script>const AUTH_KEY = '$AuthKey';</script>"; ?>
        <title>Bus Manager :: AEastwood</title>
    </head>
    <body>
        <div class="loader center"></div>
        <div class="invalid text-white center h4"></div>

        <div class="modal fade" id="notificationModal" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header bg-dark">
                        <h5 class="modal-title text-white h5">Notifications</h5>
                        <a class="h5 text-white" aria-hidden="true" class="close" data-dismiss="modal" style="cursor: pointer;">&times;</a>
                    </div>
                    <div class="modal-body">
                    <table class="notificationsTable">
                        <tbody></tbody>
                    </table>
                    </div>
                </div>
            </div>
        </div>

        <div class="d-flex justify-content-between bd-highlight mb-3 navbar-dark fixed-top bg-dark">
                <div class="p-2 bd-highlight">

                    <a class="navbar-brand text-center text-white pl-2" href="#" onclick="LoadLog();">Dashboard v1</a>
                    <a class="text-white" href="#" onclick="Reload();"><i class="fas fa-sync-alt"></i></a>
                </div>

                <div class="p-2 bd-highlight">
                    <div class="time nav-link text-white noselect"></div>
                </div>

                <div class="p-2 bd-highlight">
                    <a class="navbar-brand text-center pr-2 notifications" href="#" data-toggle="modal" data-target="#notificationModal"><i class="fas fa-bell"></i></a>
                    <a class="navbar-brand text-center pr-2" href="#" onclick="ToggleMenu();"><i class="fas fa-bars"></i></a>
                </div>
        </div>

        <div class="wrapper pr-2 pl-2">
            <div class="container-fluid">
                <div class="row pt-3">

                    <div class="col-md-9 bigCamera"></div>
                    <div class="col-md-9 cameras"></div>
                    <div class="col-md-9 log"></div>
                    <div class="col-md-9 mainDashboard">
                        <div class="card">
                            <div class="card-header bg-info text-white">Dashboard</div>
                            <div class="card-body">
                                <div class="container-fluid">
                                    <div class="col md-6"><div class="row">1</div></div>
                                    <div class="col md-6"><div class="row">2</div></div>
                                </div>
                            </div>
                        </div>

                    </div>

                    <div class="col-md-3">

                        <div class="row pb-3">
                            <div class="col-md-12">
                                <div class="card">
                                    <div class="card-header bg-dark text-white">System Status</div>
                                    <div class="card-body">
                                        <div class="container text-center systemStatus">
                                            <div class="spinner-border text-dark"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="row pb-3">
                            <div class="col-md-12">
                                <div class="card">
                                    <div class="card-header bg-info text-white">Camera Controls</div>
                                    <div class="card-body">
                                        <div class="container">
                                            <div class="row cameraIcons"></div>
                                            <div class="row cameraStorage"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row pb-3">
                            <div class="col-md-12">
                                <div class="card">
                                    <div class="card-header bg-info text-white">Door Controls</div>
                                    <div class="card-body">
                                        <table class="controls">
                                            <tbody><tr></tr></tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row pb-3">
                            <div class="col-md-12">
                                <div class="card">
                                    <div class="card-header bg-info text-white">Window Status</div>
                                    <div class="card-body">
                                        <div class="container">
                                            <div class="row windows align-items-center"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>

                </div>
            </div>

        </div>

        <script src="js/jquery-1.9.1.min.js" type="text/javascript"></script>
        <script src="js/jquery-ui-1.10.1.custom.min.js" type="text/javascript"></script>
        <script src="bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
        <script src="js/jquery.dataTables.min.js" crossorigin="anonymous"></script>

    </body>
