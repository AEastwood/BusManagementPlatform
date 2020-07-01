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
        <link type="text/css" href='http://fonts.googleapis.com/css?family=Open+Sans:400italic,600italic,400,600' rel='stylesheet'>
        <script src="https://kit.fontawesome.com/8bf1f276fe.js" crossorigin="anonymous"></script>
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.0/jquery.min.js"></script>
        <script src="js/main.js" crossorigin="anonymous"></script>
        <?php echo "<script>const AUTH_KEY = '$AuthKey';</script>"; ?>
        <title>Bus Manager :: AEastwood</title>
    </head>
    <body>
        <div class="loader center"></div>
        <div class="invalid text-white center h4"></div>

    <nav class="navbar navbar-expand-md navbar-dark fixed-top bg-dark">
        <a class="navbar-brand text-center" href="#" onclick="LoadLog();">Dashboard v1</a>
        <a class="navbar-brand text-center" href="#" onclick="Reload();"><i class="fas fa-sync-alt"></i></a>
        <ul class="navbar-nav ml-auto">
        <li class="nav-item">
            <div class="time nav-link text-white noselect"></div>
        </li>
    </ul>
    </nav>

        <div class="wrapper pr-2 pl-2">
            <div class="container-fluid">
                <div class="row pt-3">

                    <div class="col-md-9 bigCamera"></div>
                    <div class="col-md-9 cameras"></div>
                    <div class="col-md-9 log"></div>

                    <div class="col-md-3">

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
