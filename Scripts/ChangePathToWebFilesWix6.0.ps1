echo $env:UserName
$File = Get-Content "$Env:DotfuscateConfig\obfuscation_config_6.0.xml"
$XML = [XML]$File
$XPpath = "/dotfuscator/input/asmlist/inputassembly/file"

$nodes = $XML.SelectNodes($XPpath)

# Updating the attribute value for all selected nodes.
$nodes | % { $_.SetAttribute("dir", "$Env:WixOutput") }

$XPpath = "/dotfuscator/output/file"

$nodes = $XML.SelectNodes($XPpath)

# Updating the attribute value for all selected nodes.
$nodes | % { $_.SetAttribute("dir", "$Env:DotfuscateOutput") }

$XPpath = "/dotfuscator/renaming/mapping/mapoutput/file"

$nodes = $XML.SelectNodes($XPpath)

# Updating the attribute value for all selected nodes.
$nodes | % { $_.SetAttribute("dir", "$Env:DotfuscateOutput") }

# Set up formatting
$xwSettings = new-object System.Xml.XmlWriterSettings
$xwSettings.indent = $true
$xwSettings.NewLineOnAttributes = $true

# Create an XmlWriter and save the modified XML document
$xmlWriter = [Xml.XmlWriter]::Create("$Env:DotfuscateConfig\obfuscation_config_6.0.xml", $xwSettings)
$XML.Save($xmlWriter)