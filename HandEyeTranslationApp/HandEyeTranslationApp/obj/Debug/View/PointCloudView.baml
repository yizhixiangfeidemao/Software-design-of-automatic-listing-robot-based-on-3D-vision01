   M S B A M L   `   `   `  ????P  LHandEyeTranslationApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:HandEyeTranslationApp.ViewHandEyeTranslationApp.View  5      /    )HandEyeTranslationApp.View.PointCloudView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:HandEyeTranslationApp.View     5      
???????  ? DataContext  5      ??    Locator ?? Source# [   ?? Path$ 
PointCloud?.??5	      ?  ? RowDefinitions 5
   
   ?? 5      $??20 ?6   ?? 5      ?? 5      ?? 5      ?? 5      ?? 5      ?? 5      ?? 5      ?? 5      ?? 5      ?? 5       ? ColumnDefinitions 5   
   ?? 5      $??30 ?6   ?? 5      $??* ?6   .+?5   
   ??  ?? Orientation$ 
Horizontal=?6   $??0??6.   $??1??6;   .+?5      ?? $??100,0q?6   $??X??6'   $??10??60   $??16??6;   $??16(?6G   ?? 5      $??100,0q?6   $??Y??6'   $??10??60   $??16??6;   $??16(?6G   ?? 5      $??100,0q?6   $??Z??6'   $??10??60   $??16??6;   $??16(?6G   5      ?? 5    
   $??1??6   $??1??6   $??Center=?6)   $??16(?6F   ?? 5!   
   $??1??6   $??1??6"   $ 
Horizontal=?62   .+?5"      ?? -   5        $??tbxCameraX1??5"      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   ?? 5#      -   5        $??tbxCameraY1??5#      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   ?? 5$      -   5        $??tbxCameraZ1??5$      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   5%      ?? 5'   
   $??2??6   $??0??6   $??2??6-   $??Center=?69   $??16(?6V   ?? 5(   
   $??2??6   $??1??6"   $ 
Horizontal=?62   .+?5)      ?? -   5        $??tbxCameraX2??5)      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   ?? 5*      -   5        $??tbxCameraY2??5*      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   ?? 5+      -   5        $??tbxCameraZ2??5+      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   5,      ?? 5.   
   $??3??6   $??3??6   $??Center=?6)   $??16(?6F   ?? 5/   
   $??3??6   $??1??6"   $ 
Horizontal=?62   .+?50      ?? -   5        $??tbxCameraX3??50      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   ?? 51      -   5        $??tbxCameraY3??51      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   ?? 52      -	   5        $??tbxCameraZ3??52      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   53      ?? 55   
   $??4??6   $??4??6   $??Center=?6)   $??16(?6F   ?? 56   
   $??4??6   $??1??6"   $ 
Horizontal=?62   .+?57      ?? -
   5        $??tbxCameraX4??57      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   ?? 58      -   5        $??tbxCameraY4??58      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   ?? 59      -   5        $??tbxCameraZ4??59      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   5:      ?? 5<   
   $??5??6   $??5??6   $??Center=?6)   $??16(?6F   ?? 5=   
   $??5??6   $??1??6"   $ 
Horizontal=?62   .+?5>      ?? -   5        $??tbxCameraX5??5>      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   ?? 5?      -   5        $??tbxCameraY5??5?      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   ?? 5@      -   5        $??tbxCameraZ5??5@      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   5A      ?? 5C   
   $??6??6   $??6??6   $??Center=?6)   $??16(?6F   ?? 5D   
   $??6??6   $??1??6"   $ 
Horizontal=?62   .+?5E      ?? -   5        $??tbxCameraX6??5E      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   ?? 5F      -   5        $??tbxCameraY6??5F      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   ?? 5G      -   5        $??tbxCameraZ6??5G      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   5H      ?? 5J   
   $??7??6   $??7??6   $??Center=?6)   $??16(?6F   ?? 5K   
   $??7??6   $??1??6"   $ 
Horizontal=?62   .+?5L      ?? -   5        $??tbxCameraX7??5L      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   ?? 5M      -   5        $??tbxCameraY7??5M      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   ?? 5N      -   5        $??tbxCameraZ7??5N      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   5O      ?? 5Q   
   $??8??6   $??8??6   $??Center=?6)   $??16(?6F   ?? 5R   
   $??8??6   $??1??6"   $ 
Horizontal=?62   .+?5S      ?? -   5        $??tbxCameraX8??5S      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   ?? 5T      -   5        $??tbxCameraY8??5T      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   ?? 5U      -   5        $??tbxCameraZ8??5U      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   5V      ?? 5X   
   $??9??6   $??9??6   $??Center=?6)   $??16(?6F   ?? 5Y   
   $??9??6   $??1??6"   $ 
Horizontal=?62   .+?5Z      ?? -   5        $??tbxCameraX9??5Z      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   ?? 5[      -   5        $??tbxCameraY9??5[      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   ?? 5\      -   5        $??tbxCameraZ9??5\      $
??55,6q?6)   $	??100??67   $??20??6C   $??16(?6O   5]      ?? 5_   
   $??10??6   $??10??6   $??Center=?6+   $??16(?6H   ?? 5`   
   $??10??6   $??1??6#   $ 
Horizontal=?63   .+?5a      ?? -   5        $??tbxCameraX10??5a      $
??55,6q?6*   $	??100??68   $??20??6D   $??16(?6P   ?? 5b      -   5        $??tbxCameraY10??5b      $
??55,6q?6*   $	??100??68   $??20??6D   $??16(?6P   ?? 5c      -   5        $??tbxCameraZ10??5c      $
??55,6q?6*   $	??100??68   $??20??6D   $??16(?6P   5d      5f      5g      