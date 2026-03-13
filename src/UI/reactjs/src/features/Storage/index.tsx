import { Box, Container, useDrawer } from "@chakra-ui/react"
import { FileUploadDrawer, StorageHeader, StorageTable } from "./components";
import { Toaster, toaster } from "../../components/ui/toaster";
import { useStorage } from "./hooks/useStorage";
import { uploadFile, deleteFile, downloadFile } from "./services/storage-service";

export function StoragePage() {

    const { data, isLoading, error, refresh } = useStorage();

    const drawer = useDrawer();

    const handleUpload = async (files: File[]) => {

        if (!files || files.length === 0) {
            console.log("No files selected for upload.");
            return;
        }

        for (const file of files) {

            // You can add validation here if needed (e.g., file type, size)
            // For example:
            // if (file.size > MAX_FILE_SIZE) {
            //     console.error(`File "${file.name}" exceeds the maximum size limit.`);
            //     continue;
            // }

            await uploadFile(file)
                .then(() => {
                    // toast
                    refresh();
                })
                .catch((error) => {
                    console.error(`Error uploading file "${file.name}":`, error);
                    // toast
                })
        }
    }

    const handleRefresh = () => {
        console.log("Refreshing data...");
        refresh();
    }

    const handleDelete = async (id: string) => {
        await deleteFile(id)
            .then(() => {
                // toast
                refresh();
            })
            .catch((error) => {
                console.error(`Error deleting file with id "${id}":`, error);
                // toast
            });
    }

    // ref: https://bobbyhadz.com/blog/download-file-using-axios
    /* 
    Header:
    {
      Content-Type: application/octet-stream
      Content-Disposition: attachment; filename="example.jpg"
      Body: <binary stream>
    }
    */

    const handleDownloadImage = async (id: string) => {
        await downloadFile(id)
            .then((response) => {
                const href = window.URL.createObjectURL(response.data);

                const anchorElement = document.createElement("a");
                anchorElement.href = href;

                const fileName = response.headers["content-disposition"]
                    ?.split("filename=")[1]
                    .replace(/"/g, "")
                    ?? "file-download";

                anchorElement.download = fileName;

                document.body.appendChild(anchorElement);
                anchorElement.click();

                document.body.removeChild(anchorElement);
                window.URL.revokeObjectURL(href);

            })
            .catch((error) => {
                console.error(`Error downloading image with id "${id}":`, error);
            });
    }

    /* 
    Header:
    {
      Content-Disposition: inline;
    }
    */
    const handlePreview = async (id: string) => {

        await downloadFile(id)
            .then((response) => {

                const href = window.URL.createObjectURL(response.data);

                const newTab = window.open(href, "_blank");

                if (!newTab) {
                    console.error("Popup blocked.");
                    return;
                }

                // set a timeout before revoke because the new tab needs time to load the image
                setTimeout(() => {
                    window.URL.revokeObjectURL(href);
                }, 2000)

            })
            .catch((error) => {
                console.error(`Error previewing image with id "${id}":`, error);
            });
    }

    const handleSelect = (name: string) => {

    }

    return (
        <Container centerContent p={4}>
            <Box minWidth="1000px" p="4" color="black"  >
                <StorageHeader
                    onShowDrawer={() => drawer.setOpen(true)}
                    onRefresh={handleRefresh} />
                <StorageTable items={data} />
            </Box>
            <FileUploadDrawer drawer={drawer} onUpload={handleUpload} />
            <Toaster />
        </Container>
    )
}