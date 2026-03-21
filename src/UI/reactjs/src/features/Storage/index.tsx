import { Container, useDrawer } from "@chakra-ui/react"
import { FileUploadDrawer, StorageHeader, StorageTable } from "./components";
import { toaster } from "../../components/ui/toaster";
import { useStorage } from "./hooks/useStorage";
import { uploadFile, deleteFile, deleteFiles, downloadFile, getSignedUrl } from "./services/storage-service";
import { useState } from "react";

export function StoragePage() {

    const { data, refresh } = useStorage();
    const [selection, setSelection] = useState<string[]>([]);

    const drawer = useDrawer();

    // ref: https://www.chakra-ui.com/docs/components/toast#update
    const handleUpload = async (files: File[]) => {

        if (!files || files.length === 0) return;

        const toastId = toaster.create({
            title: "Uploading...",
            description: `Uploading ${files.length} file(s)`,
            type: "loading",
        });

        const results = await Promise.allSettled(
            files.map((file) => uploadFile(file))
        );

        const successCount = results.filter(r => r.status === "fulfilled").length;
        const failCount = results.filter(r => r.status === "rejected").length;

        if (successCount === files.length) {
            toaster.update(toastId, {
                title: "Success",
                description: `${successCount} file(s) uploaded`,
                type: "success",
            });
        }

        else if (failCount > 0 && successCount > 0) {
            toaster.update(toastId, {
                title: "Partial Success",
                description: `${successCount} file(s) uploaded, ${failCount} file(s) failed`,
                type: "warning",
            });
        }

        else {
            toaster.update(toastId, {
                title: "Error",
                description: `Failed to upload ${failCount} file(s)`,
                type: "error",
            });
        }

    }

    const handleRefresh = () => {
        refresh();
    }

    const handleDelete = async (id: string) => {
        await deleteFile(id)
            .then(() => {
                toaster.create({
                    description: "File deleted successfully",
                    type: "success",
                });
                refresh();
            })
            .catch(() => {
                toaster.create({
                    description: "Error deleting file",
                    type: "error",
                });
            });
    }

    const handleBulkDelete = async () => {

        if (selection.length === 0) {
            return;
        }

        await deleteFiles(selection)
            .then(() => {
                toaster.create({
                    description: "Files deleted successfully",
                    type: "success",
                });
                setSelection([]);
                refresh();
            })
            .catch(() => {
                toaster.create({
                    description: "Error deleting files",
                    type: "error",
                });
            });
    }

    // ref: https://bobbyhadz.com/blog/download-file-using-axios 
    // Header:
    // {
    //   Content-Type: application/octet-stream
    //   Content-Disposition: attachment; filename="example.jpg"
    //   Body: <binary stream>
    // }

    const handleDownloadImage = async (id: string) => {
        await downloadFile(id)
            .then((response) => {
                const href = window.URL.createObjectURL(response.data);

                const anchorElement = document.createElement("a");
                anchorElement.href = href;

                const disposition = response.headers["content-disposition"];

                let fileName = "file-download";

                if (disposition && disposition.includes("filename=")) {
                    fileName = disposition
                        .split("filename=")[1]
                        .split(";")[0]
                        .replace(/"/g, "");
                }

                anchorElement.download = fileName;

                document.body.appendChild(anchorElement);
                anchorElement.click();

                document.body.removeChild(anchorElement);
                window.URL.revokeObjectURL(href);

            })
            .catch(() => {
                toaster.create({
                    description: "Error downloading file",
                    type: "error",
                });
            });
    }

    const handlePreview = async (id: string) => {
        try {
            const response = await getSignedUrl(id);

            console.log("Signed URL:", response.data);

            if (!response.data) {
                throw new Error("Invalid signed URL");
            }

            window.open(response.data);

        } catch {
            toaster.create({
                description: "Error previewing image",
                type: "error",
            });
        }
    }

    const handleSelect = (id: string, checked: boolean) => {
        setSelection((prev) =>
            checked
                ? [...prev, id]
                : prev.filter(x => x !== id))
    }

    const handleSelectAll = (checked: boolean) => {
        setSelection(
            checked
                ? data.map(x => x.id)
                : []
        )
    }

    return (
        <>
            <Container pt={4} maxWidth="1000px" color="black">
                <StorageHeader
                    onShowDrawer={() => drawer.setOpen(true)}
                    onRefresh={handleRefresh}
                    onBulkDelete={handleBulkDelete}
                    selection={selection}
                />
                <StorageTable
                    items={data}
                    selection={selection}
                    onSelect={handleSelect}
                    onSelectAll={handleSelectAll}
                    onPreview={handlePreview}
                    onDownload={handleDownloadImage}
                    onDelete={handleDelete}
                />
                <FileUploadDrawer drawer={drawer} onUpload={handleUpload} />
            </Container>
        </>
    )
}