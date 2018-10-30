VAR player_name = "PLAYER"
VAR try_outs = 0

-> town

=== town ===
{ player_name } 站在城镇的门口
+   前往地牢[]，让我们出发吧！
    ->dungeon
    
=== dungeon ===
{ player_name } 到了地牢门口
*   进入地牢
    ->enter_dungeon
+   返回城镇
    ->return_to_town_confirm

=== return_to_town_confirm ===
确定返回城镇么？
~ try_outs++
+   { try_outs < 3 } 是的
    ->town
+   { try_outs < 3 } 不
    ->dungeon
*   { try_outs >= 3 } 想得美[，哼]，你无论如何也得给我下去！
    ->enter_dungeon

=== enter_dungeon ===
{ player_name } 进入地牢，开始探险
->END