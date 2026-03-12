import { Box, Container, useDrawer } from "@chakra-ui/react"
import { FileUploadDrawer, StorageHeader, StorageTable } from "./components";
import { useState } from "react";
import type { FileEntryModel } from "./types";
import { apiClient } from "../../lib/api-client";

export function StoragePage() {

    const dummydata: FileEntryModel[] = [
        { id: "1", name: "Laptop", description: "Electronics", uploadedAt: "2024-06-01", size: 999.99 },
        { id: "2", name: "Coffee Maker", description: "Home Appliances", uploadedAt: "2024-06-02", size: 49.99 },
        { id: "3", name: "Desk Chair", description: "Furniture", uploadedAt: "2024-06-03", size: 150.0 },
        { id: "4", name: "Smartphone", description: "Electronics", uploadedAt: "2024-06-04", size: 799.99 },
        { id: "5", name: "Headphones", description: "Accessories", uploadedAt: "2024-06-05", size: 199.99 },
    ]

    const [data, setData] = useState<FileEntryModel[]>(dummydata)

    const drawer = useDrawer();

    const onHandleUpload = async (files: File[]) => {
        for (const file of files) {

            const formData = new FormData();
            formData.append("Name", file.name)
            formData.append("Description", "")
            formData.append("FormFile", file)

            await apiClient.post(
                "/files",
                formData,
                {
                    headers: {
                        "Content-Type": "multipart/form-data"
                    }
                })
        }
    }

    const onHandleRefresh = () => {

    }

    const onHandleDelete = () => {

    }

    const onHandleDownload = () => {

    }

    const onHandlePreview = () => {

    }

    const onHandleSelect = (name: string) => {

    }

    return (
        <Container centerContent p={4}>
            <Box minWidth="1000px" p="4" color="black"  >
                <StorageHeader onShowDrawer={() => drawer.setOpen(true)} />
                <StorageTable items={data} />
            </Box>
            <FileUploadDrawer drawer={drawer} handleUpload={onHandleUpload} />
        </Container>
    )
}