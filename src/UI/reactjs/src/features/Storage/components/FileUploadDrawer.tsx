import { Box, Button, Drawer, FileUpload, Icon, Portal, CloseButton, useFileUpload, Code, Checkbox } from "@chakra-ui/react";
import type { UseDrawerReturn } from "@chakra-ui/react"
import { VscCloudUpload } from "react-icons/vsc";

type Props = {
    drawer: UseDrawerReturn,
    onUpload: (files: File[]) => void
}

export function FileUploadDrawer({ drawer, onUpload }: Props) {

    // ref: https://chakra-ui.com/docs/components/file-upload
    // ref: https://chakra-ui.com/docs/components/file-upload#store
    // you can use hook: useFileUpload() in child component,
    // here i use state for event handling
    // const [files, setFiles] = useState<File[]>([])

    const fileUpload = useFileUpload({
        maxFiles: 5,
        maxFileSize: 10 * 1024 * 1024,
        accept: [".png", ".jpg"],
        onFileAccept: (e) => {
            fileUpload.setFiles(e.files)
        }
    })

    const files = fileUpload.acceptedFiles

    return (
        <Drawer.Root
            // closeOnInteractOutside={true}
            // modal={false}
            size="md"
            open={drawer.open}
            onOpenChange={(e) => drawer.setOpen(e.open)}>
            <Portal>
                <Drawer.Positioner >
                    <Drawer.Content>
                        <Drawer.Header>
                            <Drawer.Title>Upload File</Drawer.Title>
                        </Drawer.Header>
                        <Drawer.Body>
                            <FileUpload.RootProvider
                                maxW="xl"
                                alignItems="stretch"
                                value={fileUpload}
                            >
                                <FileUpload.HiddenInput />
                                <FileUpload.Dropzone>
                                    <Icon size="2xl" color="fg.muted">
                                        <VscCloudUpload />
                                    </Icon>
                                    <FileUpload.DropzoneContent>
                                        {
                                            files.length > 0 && (
                                                <Code>{fileUpload.acceptedFiles.length} file(s) selected / 5</Code>
                                            )
                                        }
                                        <Box>Drag and drop files here</Box>
                                        <Box color="fg.muted">.png, .jpg, up to 10MB</Box>
                                    </FileUpload.DropzoneContent>
                                </FileUpload.Dropzone>
                                <FileUpload.List showSize clearable />
                            </FileUpload.RootProvider>
                        </Drawer.Body>
                        <Drawer.Footer justifyContent="flex-start" >
                            <Button
                                onClick={() => { fileUpload.clearFiles(); onUpload(files); }}
                                disabled={files.length === 0}
                            >
                                Upload
                            </Button>
                            <Drawer.ActionTrigger asChild >
                                <Button
                                    variant="outline"
                                    onClick={() => { fileUpload.clearFiles(); drawer.setOpen(false) }}>
                                    Cancel
                                </Button>
                            </Drawer.ActionTrigger>
                        </Drawer.Footer>
                        <Drawer.CloseTrigger asChild>
                            <CloseButton
                                size="sm"
                                onClick={() => { drawer.setOpen(false) }} />
                        </Drawer.CloseTrigger>
                    </Drawer.Content>
                </Drawer.Positioner>
            </Portal>
        </Drawer.Root>
    )
}