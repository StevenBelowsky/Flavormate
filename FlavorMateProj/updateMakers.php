<?php 
include("common.php");
	$link=dbConnect();

	$clientID = safe($_POST['clientID']);
	$id = safe($_POST['id']);
	$name = safe($_POST['name']);
	$isDeleting = safe($_POST['isDeleting']);
	
	$hash = safe($_POST['hash']);

    $real_hash = md5($secretKey); 

	$query = "call updateMaker('$clientID', '$id' , '$name' , '$isDeleting');"; 
	$result = mysql_query($query);    
	$my_err = mysql_error();
	
	if($result === false || $my_err != '')
	{
		echo "$my_err";
		die();
	}
	
	echo 'Added/Updated successfully';
	
	
?>