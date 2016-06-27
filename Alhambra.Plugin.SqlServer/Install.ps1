param($installPath, $toolsPath, $package, $project)

$project.ProjectItems.Item("Plugins").ProjectItems.Item("Alhambra.Plugin.SqlServer.dll").Properties.Item("CopyToOutputDirectory").Value = 2
