   M S B A M L   `   `   `  ����U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf  5
       PORLA.HMI.Module5   /PORLA.HMI.Module.Views.LogPagess.MachineLogPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Hei9http://schemas.microsoft.com/expression/2010/interactions     5      _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      T PPrism.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  5      >lvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf  5
      Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel$
  TrueX�5	         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignFont#���  5      ���#EDEDED.��5      � $	��Top=�6   .+�5   
   �� $��Log Information��6   $��13(�6,   $��0q�6:   $��0q�6N     MaterialDesignGroupBox#���  5   '   .  (MaterialDesignThemes.Wpf.ColorZoneAssist   Mode$ Custom=�5   '      
Background
 �����   
Foreground
 �����$��0��5   ^   $��2��6n   $	��850��6�   .��5      �� -   5        $��ScrollViewer1��5      $
��Auto=�5      $
��Auto=�5      .��5&      �� $��5,0,5,5q�6   $��1��6-   $��Stretch=�5'      $��Stretch=�66   ��5(      �� *AppService.LogInfors+ �� SelectedIndex 5)      �� *'%AppService.ScrollViewerVerticalOffset+ �� View 5*      �� 5+      .P�5,      ��  �� DisplayMemberBinding 6-   �� *
DateTime+��5-   "   �� 5.   &   $
��Time��60   	 �� TextAlignment$	 Center=�6<   50      �� 51      
 �� Width$	
 140��6.    6:   �� *
strLogType+��52   "   �� 53   &   $��Log Type��60   $	 Center=�6@   55      �� 56       6.   �� *
strContent+��57   "   �� 58   &   $��Description��60   $	 Center=�6C   $��NoWrap=�6Z   5:      5;      5=      5>      5?      5@      5B      