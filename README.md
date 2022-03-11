# Traffic-forwarder
An application for routing traffic through a proxy server to allow hosting on a system which cannot port-forward.

**Why not just host on the proxy server?**

Hosting something on your computer is quite convenient in certain use cases like the program having UI or wanting easier access to quickly interact with the program without connecting to the proxy server. However, the main problem being solved here is the cost of hosting. For example, if you have decent machine to host a Minecraft server (my personal reason for developing this), achieving the same level of performance with a host might cost a non-trivial amount. On the other hand, a server that can run this forwarder can be found for only a couple of dollars.

**Downsides:**
- Latency
- Less convenience than just having open ports
- Everything else that comes from hosting on your own PC (non 24/7 uptime; usually worse internet speeds; etc.)

Overall, this app is for my use-case of developing personal projects which involve networking (P2P specifically), like my [video player](https://github.com/ZenoXi/Grew) with synchronized playback.
# Usage
- Compile `LocalUI` and `Remote` projects. `Remote` can be compiled to your platform of choice, but `LocalUI` is limited to Windows since it uses WinForms. With minor modifications the project `Local` can be used instead of `LocalUI`, but it has very limited functionality.
- Run the `Remote` app on your server of choice. It requires 2 open ports: one for connecting from your local machine, the other for outside users to connect. They are respectively entered as `Host port:` and `User port`.
- Run the `LocalUI` app locally. Enter the proxy server IP and port (`Server IP` and `Server port` fields), and click connect. The `Forwarded port` field is the port on your local machine which will be routed to. It can be changed at any time while running and only affects new connections.
- Connected users are shown in the panel below, along with their public IP, local port to which they are connected, and whether the connection is idle.
- Other features like whitelisting, blacklisting, max users are self-explanatory.

**Notes**
- When someone disconnects, the entry in the connection panel might not disappear due to the nature of network programming, in this case you can close the connection manually.
- Minimizing the app moves it to the system tray to reduce clutter of the taskbar when running the app for longer periods of time.
- Since this app is a pre-pre-alpha if even that, don't expect flawless operation. Problems are fixed ad. hoc. and long-term/many-user stability and speed is not tested.

**Bugs/Questions/Feature requests**
-
These can be reported under the "Issues" tab, but I can't guarantee that all of them will be answered.
