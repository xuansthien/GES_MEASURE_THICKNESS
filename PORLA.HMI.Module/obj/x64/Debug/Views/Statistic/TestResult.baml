   M S B A M L   `   `   `  ����K  GPORLA.HMI.Module, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS.clr-namespace:PORLA.HMI.Module.Views.Statistic PORLA.HMI.Module.Views.Statistic  5      1    +PORLA.HMI.Module.Views.Statistic.TestResult   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      Eintr:http://schemas.microsoft.com/expression/2010/interactivity  5      :local.clr-namespace:PORLA.HMI.Module.Views.Statistic  5      T PPrism.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  5	      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel$
  TrueX�5
         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5         TextRenderingMode$
 Auto=�5      ���#EDEDED  MaterialDesignFont#���  5      .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   6,   �� 5      $��9* �6   6*   �� 5      $��* �6   6)   .+�5   
   �� $��0��6   $��7,0,7,5q�5      
���iii�$��	0,0,0,1.7q�5      ._�5      �� $
��Left=�6   $��Center=�5       $
��Left=�5!      $	��250��5"      $��50��5#      
������ 
�������$��13(�5&      ��5'      �� 5(      $��Center=�6!   $��Center=�5)   !    �� Orientation$ 
Horizontal=�5*   !   $��3q�5+   !   .+�5,      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ TextBoxCheckOutline=�62   $��Center=�5-   2   $��Center=�5.   2   
���   �$��40��50   2   $��40��51   2   �� 52      $�� Test Result Information��6$   $��17(�53   $   
���   �$��Center=�55   $   $��Bottom=�56   $   $��0,0,0,3q�57   $   67   58      5:      5;      �� 5<   
   $��1��6   $��0q�6   ._�5=      &   System.Windows.Controls.DataGrid     AutoGenerateColumns . $�� 0q�5?         
IsReadOnly . ���#EDEDED$��13(�5B      $	��750��5C      $
��Auto��5D      $	��Top=�5E      ��5>      �� *TestReportList+5F      5G      �� 5H   
   $��2��6   $��0,10,0,5q�5I      
���iii�$��	0,1.5,0,0q�5K      5L      � 5M   
   $��2��6   $��Center=�5N      .+�5O      �� $ 
Horizontal=�6   .+�5Q      �� $��   Start date: ��6   $��13(�5R      $��Center=�5S      6<   (  "System.Windows.Controls.DatePicker  5T      -   5        $
��dpkS��5W      $	��190��5T      $��40��5V      $��13(�5X      
���iii�
���iii�$��Center=�5[      $��Center=�5\      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 	Pick Date��5]       ( $MaterialDesignFloatingHintDatePicker#��[ 5^      	   Text	 5U      �� *	TxtFrom+
 �� Mode$
 TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=� � 	Resources 5_        PrimaryHueMidBrush �� Color��%,   &	       �� 5`      $
 Gray��5a   *   5b      5c      �� 5d      $��          End date: ��6   $��13(�5e      $��Center=�5f      6<     5g      $	��190��6   $��40��5i      $��13(�5j      
���iii�
���iii�$��Center=�5m      $��Center=�5n      $ 	Pick Date��5o      #��[ 5p      	 5h      �� *TxtTo+$
 TwoWay=�$ PropertyChanged=� 5q      ��%,   &	       �� 5r      $
 Gray��5s   *   5u      5v      �� 5w      $��          File Type: ��6   $��13(�5x      $��Center=�5y      6<   �� 5z      -   5        $��FileTypeComboBox��5z      $	��100��6�   $��40��6�   $��Center=�6�   $��Center=�6�   $��20,0,0,0q�6�   $��13(�6�   ��63   �� *	FileTypes+ �� SelectedValue 6U   �� *SelectedFileType+6�   �� 5{      
������ $��43��5}      $	��110��5~      ���#9FB6CD$��49,0,0,0q�5�      ��5|      �� *
ApplyCmd+��5�      �� 5�      $��55��6%   $��80��5�   %   $��Center=�5�   %   $��Center=�5�   %   $ 
Horizontal=�5�   %   $��3q�5�   %   .+�5�        $
 Draw=�66   $��Center=�5�   6   $��Center=�5�   6   
�������$��25��5�   6   $��25��5�   6   �� 5�      $�� Apply��6(   $��13(�5�   (   
�������$��Center=�5�   (   $��Center=�5�   (   $��0q�5�   (   65   5�      5�      �� 5�      
������ $��43��5�      $	��110��5�      ���#9FB6CD$��20,0,0,0q�5�      ��5�      �� *	ExPortCmd+��5�      �� 5�      $��55��6%   $��80��5�   %   $��Center=�5�   %   $��Center=�5�   %   $ 
Horizontal=�5�   %   $��3q�5�   %   .+�5�        $ MicrosoftExcel=�66   $��Center=�5�   6   $��Center=�5�   6   
�������$��25��5�   6   $��25��5�   6   �� 5�      $�� Export��6(   $��13(�5�   (   
�������$��Center=�5�   (   $��Center=�5�   (   $��0q�5�   (   65   5�      5�      5�      5�      5�      5�      