import { NextRequest, NextResponse } from "next/server";

export async function GET(req: NextRequest) {
    const id = req.nextUrl.searchParams.get("guid") as string;

    const message = [
        { Id: id, MessageType: "action", Room: "Living Room", Type: "lamp", Unit: "boolean", Value: 1, TimeStamp: "2022-05-01T12:00:00.000Z" },
        { Id: id, MessageType: "action", Room: "Living Room", Type: "lamp", Unit: "boolean", Value: 1, TimeStamp: "2021-05-12T12:00:00.000Z" },
        { Id: id, MessageType: "action", Room: "Living Room", Type: "lamp", Unit: "boolean", Value: 1, TimeStamp: "2021-05-01T12:00:00.000Z" }
    ];
    return NextResponse.json(message);
}
