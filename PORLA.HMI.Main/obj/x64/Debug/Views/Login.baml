   M S B A M L   `   `   `  ����@  <PORLA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:PORLA.HMI.Main.HelperPORLA.HMI.Main.Helper  5           PORLA.HMI.Main.Views.Login   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1w#clr-namespace:PORLA.HMI.Main.Helper     5      _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      [ WPrism.DryIoc.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59T PPrism.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59&prismhttp://prismlibrary.com/   5      Di:http://schemas.microsoft.com/expression/2010/interactivity   5	      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel$
  TrueX�5
         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5         TextRenderingMode$
 Auto=�5      ���#262626  MaterialDesignFont#���  5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   6)   �� 5      $
��1.5* �6   6,   �� 5      $��* �6   6)    � ColumnDefinitions 5   
   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5      $��* �6   6+   .+�5    
   �� $��1��6   $��1��5!      ���#9FB6CD$��6q�5#      ���#EDEDED$	��350��5%      $	��300��5&      ._�5'      �� .+�5(      �� $��0��6   $��	5,10,15,5q�5)      
���iii�$��	0,0,0,1.5q�5+      ._�5,      �� $
��Left=�6"   $��Center=�5-   "   $
��Left=�5.   "   $	��200��5/   "   
������ 
���   �$��17(�52   "   $�� Login��53   "   65   54      �� 55      $��
15,15,15,0q�6   
���iii�$��0,0,0,2q�57      ._�58      �� $
��Left=�6"   $��0,0,0,0q�59   "   $��Center=�5:   "   $
��Left=�5;   "   $	��150��5<   "   
���iii�
�������$��13(�5?   "   $��
 User name��5@   "   69   5A      �� 5C      -   5        $��txtUser��5E        MaterialDesignFilledTextBox#��[ 5C      $��	15,0,15,5q�5D      $	��Top=�5F      $��13(�5G       �� TextWrapping$
 Wrap=�5I      $	��700��5J      
���������5H      �� *
Username+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�5K      �� 5M      $��
15,29,15,0q�6   
���iii�$��0,0,0,2q�5O      ._�5P      �� $
��Left=�6"   $��0,0,0,0q�5Q   "   $��Center=�5R   "   $
��Left=�5S   "   $	��150��5T   "   
���iii�
�������$��13(�5W   "   $��	 Password��5X   "   68   5Y      G� 5Z      -   5         G�Name$ PasswordTextBox��5Z   !    # MaterialDesignFilledPasswordBox#��[ 5[   !   $��Center=�5\   !   $��	15,0,15,5q�5]   !   $��13(�5^   !   $��42��5_   !   $	��700��5`   !   0   *PORLA.HMI.Main.Helper.PasswordBoxAssistant	   BindPassword	 . 
�������
   BoundPassword
 5b   !   ��  �� Path$ Password�$ TwoWay=�$ PropertyChanged=�5c   6   �� 5i      $��15,5,0,0q�6   
���  ����5j      �� *Notification+5k   +   �� 5m      $��	5,10,15,5q�6   
���iii�$��	0,0,0,1.5q�5o      5q      � 5s       � Orientation$ 
Horizontal=�6   $��Center=�5t      $��0,10,0,0q�5u      .+�5v      �� -   5        $��btnLogin��5x      ���#9FB6CD$��13(�5y      $��3,3,10,3q�5z      $��45��5{      $	��120��5|       �� 	IsDefault . 
������ $��Login��5      ��5v      �� *CloseDialogCommand+5   0   �� 5�      -   5        $��btnccl��5�      ���#9FB6CD$��10,3,3,3q�5�      $��13(�5�      $��45��5�      $	��120��5�      
������ $��Cancel��5�      ��5�      �� *NavigateCommand+5�   1   5�      5�      5�      5�      5�      