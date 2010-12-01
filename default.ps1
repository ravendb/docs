properties {
  $base_dir  = resolve-path .
  $uploader = "..\Uploader\S3Uploader.exe"
  $uploadCategory = "RavenDBBook"
  $python = "\Python27\python.exe"
  $sphinx = "\Python27\Scripts\sphinx-build-script.py"
  $pdflatex_path = "\Program Files*\MiKTeX 2.*\miktex\bin\pdflatex.exe"
}

task default -depends MakePdf, StartPdf

task StartPdf {
   
  start _build\latex\RavenDBMythology-$env:buildlabel.pdf
  
}

task MakePdf -depends Init {
  $global:log = $null
  exec { 
    $global:log = &$python $sphinx -b latex -d _build/doctrees  . _build/latex
  }
  echo $global:log
  
  foreach ($line in $global:log ){
    if( $line.Contains("warning") ) {
      throw "Building the documents resulted in a warning!" 
    }
  }
  
  pushd _build/latex
  exec { 
	  $pdflatex = (ls $pdflatex_path).FullName
      & $pdflatex .\RavenDBMythology.tex
  }
  
  del RavenDBMythology-$env:buildlabel.pdf -ErrorAction SilentlyContinue
  mv RavenDBMythology.pdf RavenDBMythology-$env:buildlabel.pdf
  popd
  
}

task Init {
	if($env:buildlabel -eq $null) {
		$env:buildlabel = "00"
	}
}

task Upload -depends MakePdf {
	Write-Host "Starting upload"
	if (Test-Path $uploader) {
		$log = $env:push_msg 
		if($log -eq $null -or $log.Length -eq 0) {
		  $log = git log -n 1 --oneline		
		}
		
		$file = "_build\latex\RavenDBMythology-$env:buildlabel.pdf"
		write-host "Executing: $uploader '$uploadCategory' $file '$log'"
		&$uploader "$uploadCategory" $file "$log"
			
		if ($lastExitCode -ne 0) {
			write-host "Failed to upload to S3: $lastExitCode"
			throw "Error: Failed to publish build"
		}
	}
	else {
		Write-Host "could not find upload script $uploadScript, skipping upload"
	}	
}