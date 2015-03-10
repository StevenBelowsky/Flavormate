<?php 
include("common.php");
	$link=dbConnect();

	$clientID = safe($_POST['clientID']);
	$hash = safe($_POST['hash']);

    $real_hash = md5($password . $secretKey); 
	
	$query = "call getJuices('$clientID');"; 
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
         echo $row['Maker'] . "|" . $row['Name'] . "|" . $row['Ingredients'] . "|" . $row['Description'] . "|" . $row['JuiceID'] . "|" . $row['MakerID'] . ";";
    }
		
	
	
	
?>