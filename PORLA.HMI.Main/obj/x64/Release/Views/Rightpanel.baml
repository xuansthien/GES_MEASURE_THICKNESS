   M S B A M L   `   `   `  ���� 	  PORLA%    PORLA.HMI.Main.Views.Rightpanel   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  [ WPrism.DryIoc.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59T PPrism.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59&prismhttp://prismlibrary.com/   !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel$
  TrueX�   MaterialDesignBody#���   $��Regular"�$��13(�&   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�   TextRenderingMode$
 Auto=����#EDEDED  MaterialDesignFont#���  .���  � ColumnDefinitions �� $��* � � RowDefinitions �� $��1* ��� $
��0.5* �.+�� �� $��1��$��Bottom=�.+��� -   $��btnhelp��$��VersionPage��$��3q�$��60��$��Center=�$
��0.75q�
���iii����� *NavigateCommand+����  �� 
TargetType" �� # MaterialDesignRaisedLightButton �� BasedOn# [ .���  �� Property � �� �� Value �#D6D6D6	 �� Triggers	 �� 
 �� Binding
 �� *AppServiceInstance.pageLogin+ �� Value VersionPage.a���  � �� �#B2CAFE���� $��55��$��90��$��Center=�$��Center=� �� Orientation$ Vertical=�$��0q�.+��� $��13(����#333333$��Normal"�$��Center=�$��Center=�$��0q����� " ��.���  � ��$ Version��	 �� 
 �� *AppServiceInstance.language+ 
Vietnamese.a���  � ��$ Phiên Bản��'  !MaterialDesignThemes.Wpf.PackIcon     Kind$ InformationOutline=�$��Center=�$��Center=����#333333$��30��$��30���� -   $��btnLogin��$��AccountPage��$��3,3,3,0q�$��60��$
��0.75q�$��Center=�
���iii����� *NavigateCommand+���� " ��# [ .���  � �� �#D6D6D6	 �� 
 �� *AppServiceInstance.pageLogin+ AccountPage.a���  � �� �#B2CAFE���� $��55��$��90��$��Center=�$��Center=�$ Vertical=�$��0q�.+��� $��13(����#333333$��Normal"�$��Center=�$��Center=�$��0q����� " ��.���  � ��$
 User��	 �� 
 �� *AppServiceInstance.language+ 
Vietnamese.a���  � ��$ Tài Khoản��  $ AccountCogOutline=�$��Center=�$��Center=����#333333$��30��$��30��