   M S B A M L   `   `   `  ����   PORLA.HMI.Module6    0PORLA.HMI.Module.Views.SettingPages.SettingPanel   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     T PPrism.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel$
  TrueX����#EDEDED.��� $	��Top=� � ColumnDefinitions �� $��* �.+��� .+��� -   $��btnLogin��$��SettingPage��$��Setting��$��0,5,0,0q�$��55��
���   �$
��Thin"�$��13(�$
��0.75q�$��Center=�
���iii����� *NavigateCommand+����  �� 
TargetType" �� #  MaterialDesignRaisedLightButton �� BasedOn# [  .���  �� Property � �� �� Value �#EDEDED �� Triggers ��  �� Binding �� *!AppServiceInstance.settingPanel+ �� Value SettingPage.a���  � �� �#B2CAFE�� $��RecipeID��$��Recipe��$��0,5,0,0q�$��55��
���   �$
��Thin"�$��13(�$
��0.75q�$��Center=�
���iii����� *NavigateCommand+���� " ��# [  .���  � �� �#EDEDED ��  �� *!AppServiceInstance.settingPanel+ RecipeID.a���  � �� �#B2CAFE