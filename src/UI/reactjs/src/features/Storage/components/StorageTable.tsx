import { Checkbox, Table, Box } from "@chakra-ui/react"
import { useState } from "react"
import { StorageTableItem } from "./StorageTableItem";
import type { FileEntryModel } from "../types";

type Props = {
    items: FileEntryModel[]
}

export function StorageTable({ items }: Props) {

    const [selection, setSelection] = useState<string[]>([])
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
                                    setSelection(
                                        changes.checked ? items.map((item) => item.id) : [],
                                    )
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
                    { !items || items.length === 0 ? (
                        <Table.Row>
                            <Table.Cell colSpan={6} border="none" >No items found</Table.Cell>
                        </Table.Row>
                    ) : (
                        items.map((item) => (
                            <StorageTableItem
                                key={item.id}
                                item={item}
                                selection={selection}
                                setSelection={setSelection}
                            />
                        ))
                    )}
                </Table.Body>
            </Table.Root>
        </Box>
    )
}