param([string]$pdflatex)
psake.ps1 default.ps1 -task Upload -properties @{'pdflatex'=$pdflatex}