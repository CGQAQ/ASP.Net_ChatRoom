   M S B A M L   `   `   `  ����C  ?ChatRoom, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:ChatRoomChatRoom  5   	       ChatRoom.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:ChatRoom     5   	   -   5          � Title$  ChatRoomServer��5   	   $	��350��6    $	��525��6-    � WindowStartupLocation$ CenterScreen=�69    � 
ResizeMode$ NoResize=�6^   .��5	      � .+�5
   
   �� -   5         ��Name$ 
lblClients��5
      $��客户端列表��6$   $
��Left=�64   $��	10,10,0,0q�6O   $	��Top=�6b   �� 5   
   -   5         ��Name$ 
lbxClients��5      $
��Left=�6'   $	��185��6B   $��	10,40,0,0q�6O   $	��Top=�6b   $	��173��6z   �� 5   
   -   5        $ lbxLogs��5      $
��Left=�6#   $	��185��6>   $��
223,40,0,0q�6K   $	��Top=�6_   $	��266��6w    J� RenderTransformOrigin$ 
0.51,0.506&�6�   �� 5   
   -   5        $ lblLogs��5      $��调试输出信息��6!   $
��Left=�62   $	��Top=�6M   $��
223,10,0,0q�6e   $ 0.348,1.455&�6y   �� 5   
   -   5        $ 
lblVersion��5      $��ChatRoomSever 1.0��6$   $
��Left=�6@   $��
10,286,0,0q�6[   $	��Top=�6o   �� 5   
   -   5         ��Name$ btnOpenOrClose��5      $��启动服务器��6)   $
��Left=�69   $��401,286,0,0q�6T   $	��Top=�6i   $��88��6�   6�   �� 5   
   -   5        $ btnEnterBack��5      $��进入后台管理��6'   $
��Left=�68   $��312,286,0,0q�6S   $	��Top=�6h   $��84��6�   �� 5   
   -	   5         ��Name$ 	rectState��5      
���  ��$
��Left=�62   $��15��6M   $��
492,10,0,0q�6Y   
���   �$	��Top=�6|   $��15��6�   $ -2.043,1.136&�6�   �� 5   
   -
   5        $ lblState��5      $��服务器状态��6"   $
��Left=�62   $��	419,5,0,0q�6M   $	��Top=�6`   $ 	0.2,-0.08&�6x   5      5      