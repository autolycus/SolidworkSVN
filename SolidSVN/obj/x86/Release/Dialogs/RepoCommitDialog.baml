   M S B A M L   `   `   `  ���� C  ?SolidSVN, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:SolidSVNSolidSVN      SolidSVN.RepoCommitDialog   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     $sclr-namespace:SolidSVN       � Title$  RepoCommitDialog��$	��400��$	��400��.��� .+��� .+�]� .+��� $
��Left=�
Commiting:�� -   $��_commit_desc��$
��Left=�Files   SolidSVN.TreeList  -     Name$ 	_treeList��$	��250��$	��300��   TreeListItemNodeStyle#��[  ���� .R���  �� Orientation$ 
Horizontal=�.+�   SolidSVN.RowExpander  �� $��Bd��|�. #��z�� �� Padding# z��#��z��#��z�� J� SnapsToDevicePixels . ._��� ���� *Name+ �� Triggers ��  �� Value$
 True�� �� Binding �� *
IsSelected+ �� RelativeSource �� 	 �� AncestorType   SolidSVN.TreeListItem"	  .a��� 
 �� 
TargetName$
 Bd�� �� Property � �� �� Value C�  C� ResourceKey# Z����  � �� C� # Z���� -   $��release_locks�� d� 	IsChecked$
 TrueY�.��Release All Locks?]� .+��� -   $��commit_button��$��10,10,10,10q�$
��Left=�$��20��$��60��.��Commit�� -   $��cancel_button��$
��Left=�$��20��$��60��.��Cancel� -   $��commit_progress_bar��$��10,10,10,10q� � Value$ 0Q�