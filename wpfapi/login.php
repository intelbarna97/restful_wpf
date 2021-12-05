<?php
include("./database.php");
$request=$_SERVER["REQUEST_METHOD"];

switch($request){
    case "POST":
        if(!empty($_GET["login"]))
        {
            Login();
        }
        elseif(!empty($_GET["logout"]))
        {
            $id=$_GET["logout"];
            if(isUserLoggedIn($id))
            Logout($_GET["logout"]);
        }
        break;
        default:
		header('HTTP/1.1 405 Method Not Allowed');
		header('Allow: GET, POST, PUT, DELETE');
		break;
}


function Login()
{
    $post_vars = json_decode(file_get_contents('php://input'),true);
    $username=$post_vars["Username"];
    $password=$post_vars["Password"];

    $query="SELECT * FROM users WHERE username='".$username."' and password=SHA1('".$password."')";

    $result=classList($query);

    if(!empty($result)) {
        $response = array(
            'status' => 1,
            'status_message' => 'Login successful!'
        );
        echo json_encode(...$result);
    }
    else
    {
        $response = array(
            'status' => 0,
            'status_message' => 'Login failed! Wrong username or password!'
        );
        echo json_encode($response);
    }

    header('Content-Type: application/json');
    
}

function Logout()
{
    $response = array(
        'status' => 1,
        'status_message' => 'Logout successful!',
    );
    echo json_encode($response);
}
?>