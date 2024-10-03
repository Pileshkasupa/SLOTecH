import Message from "@/types/Message";
import { NextResponse } from "next/server";

export async function GET(): Promise<NextResponse<Message[]>> {
    return NextResponse.json([
        { Id: "Lamp", MessageType: "action", Room: "Kitchen", Type: "lamp", Unit: "boolean", Value: 1 },
        { Id: "Nanoleaf", MessageType: "action", Room: "Kitchen", Type: "lamp", Unit: "boolean", Value: 1 },
        { Id: "3", MessageType: "sensor", Room: "LivingRoom", Type: "temperature", Unit: "celsius", Value: 23 },
        { Id: "4", MessageType: "sensor", Room: "My Room", Type: "temperature", Unit: "celsius", Value: 50 },
        { Id: "5", MessageType: "action", Room: "My Room", Type: "lamp", Unit: "boolean", Value: 0 }
    ] as Message[]);
}
