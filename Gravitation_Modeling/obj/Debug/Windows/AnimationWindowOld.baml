   M S B A M L   `   `   `  ����	  WPFUI    WPFUI.AnimationWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -   5          � Title$  AnimationWindow��5   	   $	��800��6!   $
��1000��6.    � 
ResizeMode$ NoResize=�5	   	   .��5      � -   5        $��MainGrid��5       � RowDefinitions 5   
   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $	��20* �6   6+   �� 5      $��* �6   6)    � ColumnDefinitions 5   
   �� 5      $��* �6   6+   �� 5      $��2* �6   6,   �� 5      $��7* �6   6,   �� 5      $��* �6   6+   .+�5   
   �� -   5        $��PauseButton��5      $��5q�6$   $��75��6/   $��0��6:   $
��Left=�6G   $��Pause��6b   6r   �� 5   
   -   5        $��
SaveButton��5      $��5q�6$   $��75��6/   $��1��6:   $
��Left=�6G   $
��Save��6b   6q   �� 5   
   -   5        $��RewindButton��5      $��5q�6%   $��75��60   $��2��6;   $
��Left=�6H   $��Rewind��6c   {�.  6�   �� 5   
   -   5        $��
EpochLabel��5      $��1��6"   $��0��62   $��Center=�6?   $��Epoch: 0��6Z   �� 5   
   -   5        $��BodiesLabel��5      $��1��6#   $��1��63   $��Center=�6@   $��Number of bodies: 0��6[   �� 5   
   -   5        $��FPSLabel��5      $��1��6    $��2��60   $��Center=�6=   $��FPS: 0��6X   �� 5   
   
�������$��0��6%   $��3��65   $��3��6B   
���   �6V   �� 5    
   -	   5        $��FieldWidthLabel��5       $��0��6'   $��3��67   $��Width: 0��6D   $��3��6W   $��Center=�6k   $	��Top=�6�   �� 5!   
   -
   5        $��FieldHeightLabel��5!      $��0��6(   $��3��68   $��	Height: 0��6E   $��3��6Y   $
��Left=�6m   $��Center=�6�    � LayoutTransform 5"      �� 5#       �� Angle$	 270Q�6"   5%      �� 5&   
   -   5        $��
MainCanvas��5&      $��3��6#   $��3��60   }�. 6D   �� 5'   
   -   5        $��HorizontalRectangle��5'      $��0��6/   $��4��6?   $��3��6L   $��0, 10q�6`   
�������
���   �6�   �� 5(   
   -   5        $��HorizontalCanvas��5(      $��4��6)   $��3��66   }�. 6J   �� 5)   
   -   5        $��VerticalRectangle��5)      $��4��6-   $��3��6=   $��10, 0q�6J   
�������
���   �6j   �� 5*   
   -   5        $��VerticalCanvas��5*      $��3��6'   $��4��64   }�. 6D   5+      5,      