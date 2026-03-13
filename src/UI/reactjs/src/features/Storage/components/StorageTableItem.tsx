import { Box, Button, Checkbox, Menu, Portal, Table } from "@chakra-ui/react";
import { VscEllipsis, VscEye, VscTrash } from "react-icons/vsc";
import { GoMoveToBottom } from "react-icons/go";
import type { FileEntryModel } from "../types";

type Props = {
    item: FileEntryModel
    selection: string[],
    onSelect: (id: string, checked: boolean) => void
    onPreview: (id: string) => void,
    onDownload: (id: string) => void,
    onDelete: (id: string) => void,

}

export function StorageTableItem({ item, selection, onSelect, onPreview, onDownload, onDelete }: Props) {
    return (
        <Table.Row
            key={item.id}
            data-selected={selection.includes(item.id) ? "" : undefined}
        >
            <Table.Cell>
                <Checkbox.Root
                    size="sm"
                    mt="0.5"
                    aria-label="Select row"
                    checked={selection.includes(item.id)}
                    onCheckedChange={(changes) => {
                        onSelect(item.id, !!changes.checked)
                    }}
                >
                    <Checkbox.HiddenInput />
                    <Checkbox.Control />
                </Checkbox.Root>
            </Table.Cell>
            <Table.Cell>{item.name}</Table.Cell>
            <Table.Cell>{item.description}</Table.Cell>
            <Table.Cell>{item.uploadedAt}</Table.Cell>
            <Table.Cell>{item.size} kB</Table.Cell>
            <Table.Cell>
                <Menu.Root>
                    <Menu.Trigger asChild>
                        <Button bg="none" size="sm" color="black">
                            <VscEllipsis />
                        </Button>
                    </Menu.Trigger>
                    <Portal>
                        <Menu.Positioner>
                            <Menu.Content>
                                <Menu.Item value="preview">
                                    <VscEye />
                                    <Box flex="1" onClick={() => onPreview(item.id)}>Preview</Box>
                                </Menu.Item>
                                <Menu.Item value="download">
                                    <GoMoveToBottom />
                                    <Box flex="1" onClick={() => onDownload(item.id)}>Download</Box>
                                </Menu.Item>
                                <Menu.Item value="delete">
                                    <VscTrash />
                                    <Box flex="1" onClick={() => onDelete(item.id)}>Delete</Box>
                                </Menu.Item>
                            </Menu.Content>
                        </Menu.Positioner>
                    </Portal>
                </Menu.Root>
            </Table.Cell>
        </Table.Row>
    )
}