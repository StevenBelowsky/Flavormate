<?php 
include("common.php");
	$link=dbConnect();

	$password = safe($_POST['password']);
	$hash = safe($_POST['hash']);

    $real_hash = md5($password . $secretKey); 
	
	$query = "call attemptLogin('$password');"; 
	$result = mysql_query($query);    
	$my_err = mysql_error();
	
	if($result === false || $my_err != '')
	{
		echo "
		<pre>
		$my_err <br />
		$query <br />
		</pre>";
		die();
	}
	
	$num_results = mysql_num_rows($result);

    for($i = 0; $i < $num_results; $i++)
    {
         $row = mysql_fetch_array($result);
         echo $row['_id'] . ";" . $row['_name'] . ";" . $row['_logoURL'] . ";";
    }
		
	
	
	
?>