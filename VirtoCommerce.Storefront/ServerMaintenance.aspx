﻿<% Response.StatusCode = 503 %>
<!DOCTYPE html>
<html>
<head>
    <title>Maintenance</title>

    <script src="https://use.fontawesome.com/aa2b31be63.js"></script>

    <style>
        @import url('https://fonts.googleapis.com/css?family=Open+Sans:400,600,700,800');
        *
        {
            margin: 0;
            padding: 0;
        }

        html,
        body
        {
            height: 100%;
            width: 100%;
        }

        body
        {
            font-family: 'Open Sans';
        }

        h1
        {
            color: #F57C00;
            font-size: 40px;
            font-weight: 600;
            position: relative;
            text-transform: uppercase;
        }

        h1::after
        {
            color: #ae1917;
            content: '';
            font-family: "FontAwesome";
            font-size: 74px;
            left: -100px;
            position: absolute;
            top: 8px;
        }

        p
        {
            font-size: 20px;
        }

        #layout
        {
            min-height: 100%;

            display: -webkit-flex;
            display: -moz-flex;
            display: -ms-flex;
            display: -o-flex;
            display: flex;

            -webkit-align-items: center;
                    align-items: center;

            -webkit-justify-content: center;
                    justify-content: center;
        }

        .content
        {
            position: relative;
            text-align: center;
            top: -100px;
        }

        .logo
        {
            display: block;
            margin: 0 auto 20px;
        }
    </style>
</head>
<body>
    <div id="layout">
        <div class="content">
            <h1>Under Maintenance</h1>
            <p>
                The server is under maintenance, please try again later.
            </p>
        </div>
    </div>
</body>
</html>
