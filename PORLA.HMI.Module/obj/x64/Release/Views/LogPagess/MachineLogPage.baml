   M S B A M L   `   `   `  ���� U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf   PORLA.HMI.Module5   /PORLA.HMI.Module.Views.LogPagess.MachineLogPage   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Hei9http://schemas.microsoft.com/expression/2010/interactions     _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  T PPrism.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  >lvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf  Bi:http://schemas.microsoft.com/expression/2010/interactivity  !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel$
  TrueX�   MaterialDesignBody#���   $��Regular"�$��13(�&   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�   TextRenderingMode$
 Auto=�  MaterialDesignFont#���  ���#EDEDED.��� $	��Top=�.+��� $��Log Information��$��13(�$��0q�$��0q�  MaterialDesignGroupBox#���  .  (MaterialDesignThemes.Wpf.ColorZoneAssist   Mode$ Custom=�   
Background
 �����   
Foreground
 �����$��0��$��2��$	��850��.���� -   $��ScrollViewer1��$
��Auto=�$
��Auto=�.���� $��5,0,5,5q�$��1��$��Stretch=�$��Stretch=����� *AppService.LogInfors+ �� SelectedIndex �� *'%AppService.ScrollViewerVerticalOffset+ �� View �� .P���  �� DisplayMemberBinding �� *
DateTime+���� $
��Time��	 �� TextAlignment$	 Center=��� 
 �� Width$	
 140�� �� *
strLogType+���� $��Log Type��$	 Center=���  �� *
strContent+���� $��Description��$	 Center=�$��NoWrap=�