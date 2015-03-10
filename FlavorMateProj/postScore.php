<?php 
include("common.php");
	$link=dbConnect();

	$gameID = safe($_POST['gameID']);
	$name = safe($_POST['name']);
	$score = safe($_POST['score']);
	$deviceName = safe($_POST['deviceName']);
	$deviceModel = safe($_POST['deviceModel']);
	$operatingSystem = safe($_POST['operatingSystem']);
	$uniqueHashID = safe($_POST['uniqueHashID']);
	$adsRemoved = safe($_POST['adsRemoved']);
	$totalRoundsPlayed = safe($_POST['totalRoundsPlayed']);
	$numRoundsSinceBest = safe($_POST['numRoundsSinceBest']);
	
	$hash = safe($_POST['hash']);

    $real_hash = md5($name . $score . $gameID . $secretKey); 
    if($real_hash == $hash) 
	{
		if($name == "")
		{
			$name="anonymous";
		}
		$query = "INSERT INTO $dbName .`scores` (`gameID`, `name`, `score`, `deviceName`, `deviceModel`, `operatingSystem`, `uniqueHashID`, `adsRemoved`,`totalRoundsPlayed`, `numRoundsSinceBest`) 
		VALUES ($gameID, '$name', $score, '$deviceName', '$deviceModel', '$operatingSystem', '$uniqueHashID', $adsRemoved, $totalRoundsPlayed, $numRoundsSinceBest);"; 
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
		
		echo "done";
	} 
?>