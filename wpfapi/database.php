<?php 
function getConnection(){
    $server = 'localhost'; 
    $username = 'root';
    $password = '';
    $database = 'wpfapi';
    $connection = mysqli_connect($server,$username,$password, $database);
    return $connection;
}

function executeQuery($query)
{
    $connection = getConnection();
    $statement = mysqli_prepare($connection,$query);
    $success = mysqli_stmt_execute($statement);
    if($success)
    {
        echo "siker";
    }
    else{

        echo "fail: ".$query;
    }
    mysqli_stmt_close($statement);
    mysqli_close($connection);
}

function classList($query)
{
    $connection = getConnection();
    $statement = mysqli_prepare($connection,$query);
    $success = mysqli_stmt_execute($statement);
    $result = [];
    if($success === TRUE)
    {
        $idk = $statement->get_result();
        while($temp = $idk->fetch_assoc())
        {
            $result[]=$temp;
        }
    }
    else{
        echo $query;
        die('Sikertelen végrehajtás');
    }
    
    mysqli_stmt_close($statement);

    mysqli_close($connection);

    return $result;

}

function isUserLoggedIn($id) 
{
    $query="SELECT * FROM users WHERE id=".$id."";
    $result = classList($query);
	return !empty($query);
}
?>