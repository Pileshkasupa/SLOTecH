import Express from "express";
import HTTP from "http";
import { Server } from "socket.io";
const cors = require("cors");

const app = Express();
const server = HTTP.createServer(app);

app.use(cors());
app.use(Express.json());
type Message = {
    Id: string;
    Room: string;
    Value: number;
    Unit: string;
    Type: string;
    MessageType: string;
    Title?: string;
    Image?: string;
    TimeStamp?: string;
};

//@ts-ignore
const io = new Server(server, {
    cors: {
        origin: "*",
        methods: ["GET", "POST"]
    }
});
let prevConnection = undefined;
io.on("connection", (socket) => {
    console.log("client connected : " + socket.id + " " + socket.handshake.query.id);
    socket.emit(`connected`, `${socket.id}`);
    socket.on("client-ready", () => {});

    app.post("/update", async (req, res) => {
        socket.broadcast.emit("update", req.body);
        res.send("OK");
    });

    socket.conn.on("close", () => {
        prevConnection = socket.id;
        socket.conn.close();
        console.log("client disconnected : " + socket.id);
        socket.broadcast.emit("disconnected", `${prevConnection}`);
    });
});

server.listen(3050, () => {
    console.log("✔️ Server listening on port 3050");
});
