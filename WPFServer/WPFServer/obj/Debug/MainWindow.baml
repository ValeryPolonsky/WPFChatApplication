   M S B A M L   `   `   `  ����D  @WPFServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:WPFServer	WPFServer  5   	       WPFServer.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:WPFServer     5   	     � Title$  Server��5   	   $	��500��6   $
��1000��6%      LightBackground#��[  62   .��5	      � $��0,0,0,0q�6   $��Stretch=�6   $��Stretch=�69    � RowDefinitions 5
   
   �� 5      $��* �6   �� 5      $��40 �6   �� 5      $��40 �6    � ColumnDefinitions 5   
   �� 5      $	��160 �6   �� 5      $	��160 �6   �� 5      $
��150* �6   �� 5      $	��200 �6   .+�5   
   �� -   5         ��Name$ 
txtConsole��5   G   $��0��6   $��0��6#   $��3��63   $��Stretch=�6[   $��Stretch=�6y   $��
10,10,10,5q�6�   " �� VerticalScrollBarVisibility$
 Auto=�6�    �� 
IsReadOnly . $��15(�6�    � 	Resources 5       �� 
TargetType �� Property	 �� Value��%X   'J�       �� 5      " J�6   .�5      ��  �@��Margin$	 0q�6/   5      5      5      �� 5   
   -   5        $ txtConnectedUsers��5   3   $��0��6   $��3��6#   $��Stretch=�6O   $��Stretch=�6m   $��	0,10,10,5q�6�   $
 Auto=�6�    . $��15(�6�    5      ��%X   'J�       �� 5       " J�6   .�5!      ��  �@��Margin$	 0q�6/   5"      5#      5$      �� 5&   
   -   5        
 ��Name$"
 btnAcceptIncomingConnections��5&   /   $��1��6   $��0��6   $��Center=�6�   $��0,0,0,0q�6�   $��Center=�6�   $	��150��6�    J� RenderTransformOrigin$ -1.084,0.608&�6�   $��35��6�   .��5'      ��  �� TextAlignment$ Center=�6   $��13(�6/   Accept�� 6D   incoming connections6e   5(      �� 5)   
   -   5        $
 btnStopServer��5)   /   $��1��6   $��1��6   $��Stop Server��6b   $��Center=�6x   $��0,0,0,0q�6�   $��Center=�6�   $	��150��6�   $ -1.084,0.608&�6�   $��35��6�   $��13(�6�   �� 5*   
   -   5        $
 btnSendToAll��5*   .   $��2��6   $��0��6   $��Send to all��6_   $��Center=�6u   $��Center=�6�   $��0,0,0,0q�6�   $	��150��6�   $ -1.084,0.608&�6�   $��35��6�   $��13(�6�   �� 5+   
   -   5         ��Name$ 
txtMessage��5+   C   $��2��6   $��1��6   $��2��6/   $��Stretch=�6W   $��Center=�6u   $��28��6�   $��5,0,10,0q�6�    �� TextWrapping$
 Wrap=�6�   $��15(�6�   5.      5/      