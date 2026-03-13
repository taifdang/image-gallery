import { Checkbox, Table, Box } from "@chakra-ui/react"
import { StorageTableItem } from "./StorageTableItem";
import type { FileEntryModel } from "../types";

type Props = {
    items: FileEntryModel[],
    selection: string[],
    onSelect: (id: string, checked: boolean) => void,
    onSelectAll: (checked: boolean) => void,
    onPreview: (id: string) => void,
    onDownload: (id: string) => void,
    onDelete: (id: string) => void
}

export function StorageTable({ items, selection, onSelect, onSelectAll, onPreview, onDownload, onDelete }: Props) {

    const hasSelection = selection.length > 0
    const indeterminate = selection.length > 0 && selection.length < items.length

    return (
        <Box paddingTop="5">
            <Table.Root>
                <Table.Header>
                    <Table.Row>
                        <Table.ColumnHeader w="6">
                            <Checkbox.Root
                                size="sm"
                                mt="0.5"
                                aria-label="Select all rows"
                                checked={indeterminate ? "indeterminate" : selection.length > 0}
                                onCheckedChange={(changes) => {
                                    onSelectAll(!!changes.checked)
                                }}
                            >
                                <Checkbox.HiddenInput />
                                <Checkbox.Control />
                            </Checkbox.Root>
                        </Table.ColumnHeader>
                        <Table.ColumnHeader>Name</Table.ColumnHeader>
                        <Table.ColumnHeader>Description</Table.ColumnHeader>
                        <Table.ColumnHeader>Uploaded At</Table.ColumnHeader>
                        <Table.ColumnHeader>Size</Table.ColumnHeader>
                        <Table.ColumnHeader></Table.ColumnHeader>
                    </Table.Row>
                </Table.Header>
                <Table.Body>
                    {!items || items.length === 0 ? (
                        <Table.Row>
                            <Table.Cell colSpan={6} border="none" >No items found</Table.Cell>
                        </Table.Row>
                    ) : (
                        items.map((item) => (
                            <StorageTableItem
                                key={item.id}
                                item={item}
                                selection={selection}
                                onSelect={onSelect}
                                onPreview={onPreview}
                                onDownload={onDownload}
                                onDelete={onDelete}
                            />
                        ))
                    )}
                </Table.Body>
            </Table.Root>
        </Box>
    )
}