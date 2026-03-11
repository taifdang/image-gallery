import { Box, Button, Checkbox, Menu, Portal, Table } from "@chakra-ui/react";
import { VscEllipsis, VscEye, VscTrash } from "react-icons/vsc";
import { GoMoveToBottom } from "react-icons/go";
import type { FileEntryModel } from "../types";

type Props = {
    item: FileEntryModel
    selection: string[]
    setSelection: React.Dispatch<React.SetStateAction<string[]>>
}

export function StorageTableItem({ item, selection, setSelection }: Props) {
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
                        setSelection((prev) =>
                            changes.checked
                                ? [...prev, item.id]
                                : selection.filter((id) => id !== item.id),
                        )
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
                                    <Box flex="1">Preview</Box>
                                </Menu.Item>
                                <Menu.Item value="download">
                                    <GoMoveToBottom />
                                    <Box flex="1">Download</Box>
                                </Menu.Item>
                                <Menu.Item value="delete">
                                    <VscTrash />
                                    <Box flex="1">Delete</Box>
                                </Menu.Item>
                            </Menu.Content>
                        </Menu.Positioner>
                    </Portal>
                </Menu.Root>
            </Table.Cell>
        </Table.Row>
    )
}