   M S B A M L   `   `   `  ���� 	  PORLA%    PORLA.HMI.Main.Views.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     [ WPrism.DryIoc.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59T PPrism.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59&prismhttp://prismlibrary.com/   !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel$
  TrueX� � WindowStartupLocation$ CenterScreen=� � WindowState$ 	Maximized=� � WindowStyle$
 None=� � 
ResizeMode$ NoResize=� � Title$ PORLA��$
��1024��$
��1280��.���  � RowDefinitions �� $
��1.3* ��� $	��12* ��� $
��0.9* �.+��� $��0�����#414141$��0q�._��� $��0��!  Prism.Regions.RegionManager   
RegionName$ TopPanelRegion���� $��2�����#414141$��0q�._��� $ BottomPanelRegion��� $��1�� � ColumnDefinitions �� $��11.5* ��� $	��120 � �� $
��0.7* ��� $��* �.+��� $��0��$��2��
���iii�$��1q�$��3,2,3,0q����#D6D6D6._��� $ ContentRegion���� $��1��$��0��
���iii�$��1,1,1,0q�$��1,2,3,0q����#D6D6D6._��� $ OptionPanelRegion���� $��1��$��1��
���iii�$��1,0,1,1q�$��1,0,3,0q����#D6D6D6._��� $ RightPanelRegion��