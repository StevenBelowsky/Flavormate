<?php 
include("common.php");
	$link=dbConnect();

	$clientID = safe($_POST['clientID']);
	$id = safe($_POST['id']);
	$name = safe($_POST['name']);
	$description = safe($_POST['description']);
	$makerID = safe($_POST['makerID']);
	$ingredients = safe($_POST['ingredients']);
	$isDeleting = safe($_POST['isDeleting']);
	
	$hash = safe($_POST['hash']);

    $real_hash = md5($secretKey); 

	$query = "call updateJuice('$clientID', '$id' , '$name' , '$description' , '$makerID' , '$ingredients' , '$isDeleting');"; 
	$result = mysql_query($query);    
	$my_err = mysql_error();
	
	if($result === false || $my_err != '')
	{
		echo "$my_err";
		die();
	}
	
	echo 'Added/Updated successfully';
	
	
?>