import { NextRequest, NextResponse } from "next/server";
import Message from "@/types/Message";
import fs from "fs";
import path from "path";

const filePath = path.join(process.cwd() + "/public/components/declaredComponents.json");

export async function GET(req: NextRequest) {
    const id = req.nextUrl.searchParams.get("guid") as string;
    const fileData = fs.readFileSync(filePath, "utf8");

    const data = JSON.parse(fileData);

    const result = data.find((item: any) => item.Id.toString() === id);

    if (result) {
        return NextResponse.json(JSON.stringify(result), {
            status: 200,
            headers: {
                "Content-Type": "application/json"
            }
        });
    } else {
        return NextResponse.json(JSON.stringify({ error: "Item not found" }), {
            status: 404,
            headers: {
                "Content-Type": "application/json"
            }
        });
    }
}

export async function POST(req: NextRequest) {
    const data = (await req.json()) as Message;
    let fileData = fs.readFileSync(filePath, "utf8");
    let dataArray;
    // Check if fileData is not empty and is valid JSON
    if (fileData) {
        dataArray = JSON.parse(fileData);
    }

    // If dataArray is not an array, initialize it as one
    if (!Array.isArray(dataArray)) {
        dataArray = [];
    }

    const exists = dataArray.some((item: Message) => item.Id === data.Id); // Adjust the condition according to your data

    if (!exists) {
        dataArray.push(data);

        // Write the updated array back to the file
        fs.writeFileSync(filePath, JSON.stringify(dataArray, null, 2)); // null and 2 for pretty formatting
    } else {
        return NextResponse.json("Component with that ID already exists");
    }
    return NextResponse.json(JSON.stringify("Device added"));
}

export async function DELETE(req: NextRequest) {
    const id = req.nextUrl.searchParams.get("guid") as string;
    const fileData = fs.readFileSync(filePath, "utf8");
    const data = JSON.parse(fileData);
    const updatedDevices = data.filter((device: any) => device.Id !== id);
    fs.writeFileSync(filePath, JSON.stringify(updatedDevices, null, 2), "utf8");
    return NextResponse.json("OK");
}
