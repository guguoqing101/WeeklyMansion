select * from RoomStatuses;
select * from Reservations;

SELECT PropertyId, RoomNo, COUNT(*) AS Count
FROM Rooms
GROUP BY PropertyId, RoomNo
HAVING COUNT(*) > 1;

-- truncate table Reservations;
-- truncate table RoomStatuses;

/* 
Rooms.Status:
0 = false(利用停止)
1 = true(利用可能)

RoomStatuses.Status:
0 = 空室
1 = 予約済
2 = 清掃中
3 = メンテナンス
*/



