# signalR
app mvc + signalR service + windows service data
1. SignalR API Service: 
 - Connection manager
 - Send/Receive message from client
 - Receive message from windows service and send to client or group(channel)
2. App MVC: client browser
	- chat
	- priceboard	
3. Windows service(priceboardservice):
 - connect signalR API and send data