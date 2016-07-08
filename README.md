WPF ResMerger
=============

We all agree that a good styling structure is essential for a successful WPF project. However, while we should certainly stick to the granular structure dividing each type into different resource dictionaries, dependencies between merged dictionaries will grow tremendously. This overhead can slow down the loading time of control heavy views a lot. And exactly this is the reason why we developed a tool which solves this very problem... The ResMerger!

The ResMerger generates one big resource dictionary at build time. 
Simply include this big resource dictionary inside your App.xaml and feel a real performance boost!

Howto
----------
* Build the ResMerger project with Visual Studio
* Copy the ResMerger.exe to your solution folder (Outside Visual Studio)
* Maybe create a subfolder called Tools
* Edit the pre build event of your styling project and add a call to the ResMeger.exe
  * "$(SolutionDir)Tools\ResMerger\ResMerger.exe" "$(ProjectDir)\" $(ProjectName) "/Source.xaml" "/Output.xaml"
  * Don't miss the "" signs

![picture alt](http://www.davidchristian.de/images/prebuild.png "Prebuild")

Parameters of the ResMerger
----------
* "$(ProjectDir)\" describes the styling project dir
* $(ProjectName) describes the project name (optional, default the last folder name of the project dir)
* "/Source.xaml" describes the source resource dictionary with all merged ones (optional, default LookAndFeel.xaml)
* "/Output.xaml" describes the output of the awesome output of the ResMerger (optional, default FullLookAndFeel.xaml)

Styling Project?
----------
The styling project contains all WPF resources like colors, brushes, styles and control templates.
Typically, all resources are distributed over many resource dictionaries and there is one big resource dictionary LookAndFeel.xaml.

![picture alt](http://www.davidchristian.de/images/stylingproject.png "Styling project")

FullLookAndFeel.xaml (Output)
----------
Another thing you might want to do is, to mark the output resource dictionary dependent upon the source file! (which really kills it). To do this just edit the project file like this:

![picture alt](http://www.davidchristian.de/images/dep.png "FullLookAndFeel")

Feedback
----------
So hopefully you like it and share your impressions with us! André Lanninger & David C. Thömmes

License
----------
All source code is licensed under the [MIT License](https://raw.githubusercontent.com/dctdct/WPF-ResMerger/master/LICENSE.txt).
