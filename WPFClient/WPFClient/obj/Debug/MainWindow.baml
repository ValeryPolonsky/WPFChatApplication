   M S B A M L   `   `   `  ����D  @WPFClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:WPFClient	WPFClient  5   	       WPFClient.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:WPFClient     5   	     � Title$  Client��5   	   $	��500��6   $
��1000��6%      LightBackground#��[  62   .��5      � $��Stretch=�6   $��Stretch=�6)    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��35 �6   �� 5      $��35 �6    � ColumnDefinitions 5   
   �� 5      $	��110 �6   �� 5      $
��310* �6   �� 5      $
��150* �6   �� 5      $	��200 �6   .+�5   
   �� -   5         ��Name$ txtMessagesList��5   G   $��3��6   $��0��6*   $��0��67   $��Stretch=�6`   $��Stretch=�6~   $��	10,10,0,5q�6�   $
��Auto=�6�    �� 
IsReadOnly . $��15(�6�    � 	Resources 5       �� 
TargetType �� Property �� Value��%X   'J�       �� 5      " J�6   .�5      ��  �@��Margin$ 0q�6/   5      5      5      �� 5   
   -   5        $ txtConnectedUsers��5   G   $��3��6   $��0��6*   $��3��67   $��Stretch=�6b   $��Stretch=�6�   $��
10,10,10,5q�6�   $
��Auto=�6�    . $��15(�6�    5       ��%X   'J�       �� 5!      " J�6   .�5"      ��  �@��Margin$ 0q�6/   5#      5$      5%      �� 5'   
   -   5        	 ��Name$	 btnSendMessage��5'   /   $��1��6   $��0��6   $��Send message��6d   $��10,0,0,0q�6|   $��Center=�6�   $��90��6�   $��23��6�   
 J� RenderTransformOrigin$
 -0.294,0.06&�6�   $��13(�6�   �� 5(   
   -   5         ��Name$ 
txtMessage��5(   C   $��2��6   $��1��6&   $��1��63   $��Stretch=�6W   $��23��6u   $��0,0,0,0q�6�    �� TextWrapping$
 Wrap=�6�   $��Center=�6�   $��13(�6�   �� 5*   
   -   5        $	 
btnSetName��5*   /   $��2��6   $��0��6   $��Set name��6]   $��10,0,0,0q�6q   $��Center=�6�   $��90��6�   $��23��6�   $
 -0.294,0.06&�6�   $��13(�6�   �� 5+   
   -   5        $ txtName��5+   C   $��2��6   $��2��6   $��1��63   $��Stretch=�6T   $��23��6r   $��0,0,0,0q�6~   $
 Wrap=�6�   $��Center=�6�   $��13(�6�   5.      5/      