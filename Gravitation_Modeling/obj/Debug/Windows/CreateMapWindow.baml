   M S B A M L   `   `   `  ����@  <WPFUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:WPFUI.ControlsWPFUI.Controls  5   	   #    WPFUI.Windows.CreateMapWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1controlsclr-namespace:WPFUI.Controls     5   	   -   5          � Title$  CreateMapWindow��5   	   $	��800��6!   $
��1000��6.    � WindowStartupLocation$ CenterScreen=�5	   	    � 
ResizeMode$ CanMinimize=�5
   	   .��5      �  � ColumnDefinitions 5   
   �� 5      $��* �6   6+   �� 5      $	��15* �6   6-   �� 5      $��* �6   6+   �� 5      $��5* �6   6,    � RowDefinitions 5   
   �� 5      $��* �6   6)   �� 5      $	��20* �6   6+   �� 5      $��* �6   6)   .+�5   
   �� -   5        $��1��5      $��1��6$   
�������
���   �6Q   �� 5   
   -   5        $��
MainCanvas��5      $��1��6#   $��1��63   }�. 6@   o� 5   
   $��0��6   $��1��6   $��2��6,   .@�5      �� -   5         ��Name$ NewBodyButton��5      $��30��6,   $��30��67   .��5      �� $/��)pack://application:,,,/Resources/Body.png��6   5      5      o� 5    
   $��0��6   $��0��6   $��3��6,   .@�5!      �� -   5        $��SaveMapButton��5!      $��30��6*   $��30��65   .��5"      �� $/��)pack://application:,,,/Resources/Save.png��6   5#      �� 5$      -   5        $��PlayMapButton��5$      $
��Play��6*   {�.  69   5%      �� 5&   
   -   5        $��
WidthLabel��5&      $��Width: 0��6"   $��1��65   $��1��6E   $��Center=�6R   $	��Top=�6p   �� 5'   
   -   5        $��HeightLabel��5'      $��	Height: 0��6#   $��1��67   $��1��6G   $
��Left=�6T   $��Center=�6p    � LayoutTransform 5(      �� 5)       �� Angle$	 270Q�6"   5+      �� 5-   
   $��3��6   $��0��6$   $��3��61   
���   �$��3��6Q   !   WPFUI.Controls.PropertyMenu  5.   
   -	   5          Name$ MainPropertyMenu��5.       $��3��6:   $��0��6J   $��3��6W   5/      50      