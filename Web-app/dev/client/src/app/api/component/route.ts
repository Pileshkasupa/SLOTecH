import Message from "@/types/Message";
import { NextRequest, NextResponse } from "next/server";

export async function GET(req: NextRequest) {
    const id = req.nextUrl.searchParams.get("guid") as string;
    const devices = [
        { Id: "Lamp", MessageType: "action", Room: "Kitchen", Type: "lamp", Unit: "boolean", Value: 1 },
        { Id: "Nanoleaf", MessageType: "action", Room: "Kitchen", Type: "lamp", Unit: "boolean", Value: 1 },
        { Id: "3", MessageType: "sensor", Room: "LivingRoom", Type: "temperature", Unit: "celsius", Value: 23 },
        { Id: "4", MessageType: "sensor", Room: "My Room", Type: "temperature", Unit: "celsius", Value: 50 },
        { Id: "5", MessageType: "action", Room: "My Room", Type: "lamp", Unit: "boolean", Value: 0 }
    ];
    const message = devices.find((device) => device.Id === id) as Message;
    const messageArray = [message];
    return NextResponse.json(messageArray);
}
