   M S B A M L   `   `   `  ????P  LHandEyeTranslationApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:HandEyeTranslationAppHandEyeTranslationApp  5   	   &     HandEyeTranslationApp.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:HandEyeTranslationApp     5   	     ? Title$  HandEyeCalibration??5   	   $??50(?6$   $	??500??63   $	??800??6@    ? WindowStartupLocation$ CenterScreen=?6L   
???????.??5	      ?  ? ColumnDefinitions 5
   
   ?? 5      $	??100 ?6   ?? 5      $??* ?6   .+?5   
   ?? ???#22226#   ?? 5   
   $??0,0,0,0q?6   
?????? 
???   ?$??0q?6O   $??16(?6c   ??5      ?? 5       ?? 
TargetType" ??6   .?5      ??  ?? Property ? ?? ?? Value 5      ?? 5       ?? 
TargetType" ??6.   .R?5   "   ?? $??	Container??6k   $??60??6-    ?? Orientation$ 
Horizontal=?69   
??????  ? Cursor$
 Hand??6|   .+?5   &   ?? $??LeftSideRectangle??6w   
???   ?$??Stretch=?6=   $??5??6Y   $y?Hidden=?6c   6?   ?? 5   &   $??Center=?67   $??15,0,0,0q?6R   5   #   	 ?? Triggers	 5   "   P? 5   &   
 P? Property
 ?@J?IsMouseOver P? Value . . ?5   *   ??  ?? 
TargetName$ LeftSideRectangle??6W    ? y?$ Visible=?6G   6W   ?? 5   *   $ 	Container??6V    ? ?? ?#11115    '   P? 5!   &   
 ?@??
IsSelected . . ?5"   *   ?? $ LeftSideRectangle??6W    ? y?$ Visible=?6G   6W   ?? 5#   *   $ 	Container??6U    ? ?? ?#44445$   '   5'      5)      5*      .@?5-      ?? -   5        $??相机??5-      6Z   ?? 5.      -   5        $??点云??5.      6[   ?? 5/      -   5        $??	机器人??5/      6\   ?? 50      -   5        $??转换??50      6[   ?? 51      -   5        $??关于??51      6\   53      ? 57   
   $??1??6   .+?59      ?? -   5         ??Name$ 	container??59      5;      5>      5?      