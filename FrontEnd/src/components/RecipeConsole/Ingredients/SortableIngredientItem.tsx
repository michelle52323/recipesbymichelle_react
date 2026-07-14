import React, { useState, useEffect } from "react";
import { useSortable } from "@dnd-kit/sortable";
import { useDndContext } from '@dnd-kit/core';
import { CSS } from "@dnd-kit/utilities";
import Icon from "../../UserControls/Icons/icons";
import type { Ingredient } from "../../../types/Recipe/Recipe";
import type { MeasurementUnit } from "src/types/Measurement/MeasurementType";
import {
    trimQuantity, validateUnitInput, requiresPlural,
    renderNumberDisplayBySystem, renderNumberDisplayRange, renderNumberDisplayRangeFromString, getAbbreviation,
    parseQtyInput
} from "../../../helpers/measurementHelper";



interface Props {
    ingredient: Ingredient | null;
    index: number;

    // Normal row
    handleSave?: (ingredient: Ingredient) => void;
    openDeleteModal?: () => void;

    // Add row
    isAddRow?: boolean;
    handleAdd?: (added: Ingredient) => void;

    deviceType: "desktop" | "mobile";

    recentlySavedId: number | null;
    setAddRow?: React.Dispatch<React.SetStateAction<Ingredient>>;

    // NEW: validation + pending action
    openValidationModal: (
        errors: string[],
        action: "add" | "save",
        ingredient: Ingredient
    ) => void;

    pendingAction: "add" | "save" | null;
    setPendingAction: React.Dispatch<React.SetStateAction<"add" | "save" | null>>;

    unitLookupTable: MeasurementUnit[];
    measurementSystem: "Imperial" | "Metric" | null;

    isOpen?: boolean;
    openId?: string | null;
    setOpenId?: React.Dispatch<React.SetStateAction<string | null>>;
    onToggle?: () => void;

}


const SortableIngredientItem: React.FC<Props> = ({
    ingredient,
    index,
    handleSave,
    handleAdd,
    openDeleteModal,
    isAddRow = false,
    deviceType,
    recentlySavedId,
    setAddRow,
    openValidationModal,
    pendingAction,
    setPendingAction,
    unitLookupTable,
    measurementSystem,
    isOpen,
    openId,
    setOpenId,
    onToggle
}) => {
    //console.log("render lookup:", unitLookupTable);
    //console.log("render measurement system:", measurementSystem);

    // Local editable state
    // For normal rows, keep local state.
    // For Add Row, derive values from parent-controlled ingredient.
    const [localQty, setLocalQty] = useState(ingredient?.quantity ?? "");
    const [localQtyMax, setLocalQtyMax] = useState(ingredient?.quantityMax ?? "");
    const [localUnit, setLocalUnit] = useState(ingredient?.unit ?? "");
    const [localDesc, setLocalDesc] = useState(ingredient?.description ?? "");
    const [localInstr, setLocalInstr] = useState(ingredient?.instructions ?? "");

    // Unified values used by the UI
    const qty = isAddRow ? ingredient?.quantity ?? "" : localQty;
    //const qty = isAddRow ? ingredient!.quantity : localQty;

    const qtyMax = isAddRow ? ingredient?.quantityMax ?? "" : localQtyMax;
    //console.log("Qty: " + qty + " to Qty Max: " + qtyMax);

    const [localQtyInput, setLocalQtyInput] = useState(
        ingredient?.quantityMax
            ? `${ingredient.quantity}-${ingredient.quantityMax}`
            : ingredient?.quantity ?? ""
    );


    useEffect(() => {
        if (isAddRow) {
            const rebuilt = ingredient?.quantityMax
                ? `${ingredient.quantity}-${ingredient.quantityMax}`
                : ingredient?.quantity ?? "";

            setLocalQtyInput(rebuilt);
        }
    }, [
        isAddRow,
        ingredient?.quantity,
        ingredient?.quantityMax
    ]);


    const unit = isAddRow ? ingredient?.unit ?? "" : localUnit;
    const desc = isAddRow ? ingredient?.description ?? "" : localDesc;
    const instr = isAddRow ? ingredient?.instructions ?? "" : localInstr;




    const qtyRef = React.useRef<HTMLInputElement>(null);
    const isHighlighted = !isAddRow && ingredient?.id === recentlySavedId;

    const sortable = !isAddRow
        ? useSortable({ id: ingredient!.id.toString() })
        : {
            attributes: {},
            listeners: {},
            setNodeRef: () => { },
            transform: null,
            transition: null
        };

    const { attributes, listeners, setNodeRef, transform, transition } = sortable;


    const style = {
        transform: CSS.Transform.toString(transform),
        transition,
    };

    const onSave = () => {
        // 1. Clean quantity
        const cleanedQty = trimQuantity(qty);
        const cleanedQtyMax = trimQuantity(qtyMax);

        const qtyForPlural = cleanedQtyMax || cleanedQty;

        // 2. Plural logic
        const isPlural = requiresPlural(qtyForPlural, measurementSystem);

        // 3. Validate + clean unit
        const result = validateUnitInput(
            measurementSystem,
            unit,
            isPlural,
            unitLookupTable
        );
        const cleanedUnit = result.cleaned.trim();

        // 4. Clean description + instructions
        const cleanedDesc = desc.trim();
        const cleanedInstr = instr.trim();

        // 5. Update local state so UI reflects cleaned values
        setLocalQty(cleanedQty);
        setLocalQtyMax(cleanedQtyMax);
        setLocalUnit(cleanedUnit);
        setLocalDesc(cleanedDesc);
        setLocalInstr(cleanedInstr);

        // 6. Perform save with cleaned values
        handleSave?.({
            ...ingredient!,
            quantity: cleanedQty,
            quantityMax: cleanedQtyMax,
            unit: cleanedUnit,
            description: cleanedDesc,
            instructions: cleanedInstr
        });
    };



    const onAdd = () => {
        setAddRow(prev => {
            const cleanedQty = trimQuantity(prev.quantity);
            const cleanedQtyMax = trimQuantity(prev.quantityMax);

            const qtyForPlural = cleanedQtyMax || cleanedQty;
            const isPlural = requiresPlural(qtyForPlural, measurementSystem);

            const result = validateUnitInput(
                measurementSystem,
                prev.unit,
                isPlural,
                unitLookupTable
            );

            const cleanedIngredient = {
                ...prev,
                quantity: cleanedQty,
                quantityMax: cleanedQtyMax,
                unit: result.cleaned.trim(),
                description: prev.description.trim(),
                instructions: prev.instructions.trim(),
                id: 0,
                sortOrder: index + 1,
                isActive: true
            };

            handleAdd?.(cleanedIngredient);

            return cleanedIngredient;
        });
    };


    useEffect(() => {
        if (isAddRow) {
            const isBlank =
                (ingredient?.quantity ?? "") === "" &&
                (ingredient?.quantityMax ?? "") === "" &&
                (ingredient?.unit ?? "") === "" &&
                (ingredient?.description ?? "") === "" &&
                (ingredient?.instructions ?? "") === "";

            if (isBlank) {
                qtyRef.current?.focus();
            }
        }
    }, [
        isAddRow,
        ingredient?.quantity,
        ingredient?.quantityMax,
        ingredient?.unit,
        ingredient?.description,
        ingredient?.instructions
    ]);


    useEffect(() => {

        if (!isAddRow && ingredient?.id === recentlySavedId) {
            qtyRef.current?.focus();
        }
    }, [recentlySavedId, isAddRow, ingredient]);

    const { active } = useDndContext();
    const isDragging = active?.id === ingredient.id.toString();

    const expandedClass = isOpen ? "expanded-content-tint" : "";
    const draggingClass = isDragging ? "drag-item-tint" : "";

    if (deviceType === "mobile") {
        return (
            <>
                <div
                    ref={setNodeRef}
                    style={style}
                    className={`mobile-ingredient-row grid-page-row ${expandedClass}  ${draggingClass}`}

                >
                    {/* COLLAPSED ROW ADD ROW */}
                    {(isAddRow && !isOpen) && (
                        <>
                            <div
                                className=" add-item-row d-flex align-items-center"
                                onClick={() => onToggle()}

                                style={{
                                    cursor: "pointer",
                                    padding: "8px 16px",
                                }}
                            >
                                <div className="d-flex align-items-center">
                                    <div className="add-icon me-3">
                                        <Icon name="add" />
                                    </div>
                                </div>

                                <div className="flex-grow-1">
                                    <span className="fw-bold">Add Ingredient</span>
                                </div>
                            </div></>
                    )}
                    {/* COLLAPSED ROW EDITABLE ROW */}
                    {(!isAddRow || isOpen) && (
                        <>
                            <div className="d-flex align-items-start mobile-collapsed-row">

                                {/* Drag handle column */}
                                <div className="drag-handle-column">
                                    <div className="mobile-drag-handle-wrapper">
                                        {!isAddRow ? (
                                            <div className="mobile-drag-handle" {...listeners}>
                                                <Icon name="drag" />
                                            </div>
                                        ) : (
                                            <div className="mobile-drag-handle">&nbsp;</div>
                                        )}
                                    </div>
                                </div>

                                {/* Content column */}
                                <div className="content-column flex-grow-1">
                                    <div className="row mobile-content-col-height">

                                        {/* Qty + Unit */}
                                        <div className="col-4">
                                            <div className="d-flex align-items-baseline flex-wrap text-14 ">
                                                {/* Plain text for now — formatting helpers later */}
                                                {/* <span
                                                    dangerouslySetInnerHTML={{
                                                        __html: renderNumberDisplayRange(
                                                            qty.toString(),
                                                            qtyMax.toString(),
                                                            measurementSystem,
                                                            "SansSerif"
                                                        ),
                                                    }}
                                                /> */}
                                                <span
                                                    dangerouslySetInnerHTML={{
                                                        __html: renderNumberDisplayRangeFromString(
                                                            localQtyInput.toString(),
                                                            measurementSystem,
                                                            "SansSerif"
                                                        ),
                                                    }}
                                                />

                                                <span className="ps-1">{getAbbreviation(unit, unitLookupTable)}</span>
                                            </div>
                                        </div>

                                        {/* Description */}
                                        <div className="col-8 ps-3 ">
                                            <span className="truncate-one-line">{desc}</span>
                                        </div>
                                    </div>
                                </div>

                                {/* Action column */}
                                <div className="action-column">
                                    <div className="fixed-button">
                                        <button
                                            type="button"
                                            className="button button-icon"
                                            onClick={() => onToggle()}
                                        >
                                            {isOpen ? (
                                                <div className="margin-3"><Icon name="chevronUp" /></div>
                                            ) : (
                                                <div className="margin-4"><Icon name="pencil" /></div>
                                            )}
                                        </button>
                                    </div>
                                </div>
                            </div></>
                    )}


                    {/* EXPANDED AREA */}
                    {isOpen && (
                        <>
                            <div className="pt-1 accordion-content expanded-content" data-expand-id={isAddRow ? "ADD_ROW" : ingredient.id}>
                                <div className="p-2 d-flex flex-wrap accordion">
                                    <div className="accordion-content-inner ps-3 pe-3">

                                        {/* Qty */}
                                        <div className="fixed-textbox-large qty-input height-65">
                                            <div className="label-mobile">Quantity</div>
                                            <div className="form-element">
                                                <input
                                                    ref={qtyRef}
                                                    type="text"
                                                    className="form-control textbox textbox-small textbox-text"
                                                    maxLength={20}
                                                    value={localQtyInput}
                                                    // onChange={(e) => {
                                                    //     if (isAddRow) {
                                                    //         setAddRow?.(prev => ({ ...prev, quantity: e.target.value }));
                                                    //     } else {
                                                    //         setLocalQty(e.target.value);
                                                    //     }
                                                    // }}


                                                    // onBlur={(e) => {
                                                    //     const cleanedQty = trimQuantity(e.target.value);

                                                    //     // Update DOM
                                                    //     e.target.value = cleanedQty;

                                                    //     // 1. Update qty state
                                                    //     if (isAddRow && cleanedQty !== qty) {
                                                    //         setAddRow(prev => ({ ...prev, quantity: cleanedQty }));
                                                    //     }

                                                    //     if (!isAddRow && cleanedQty !== localQty) {
                                                    //         setLocalQty(cleanedQty);
                                                    //     }

                                                    //     // 2. Recalculate pluralization using the cleaned qty
                                                    //     const isPlural = requiresPlural(cleanedQty, measurementSystem);

                                                    //     // 3. Re-validate + autocorrect the unit
                                                    //     const result = validateUnitInput(
                                                    //         measurementSystem,
                                                    //         isAddRow ? unit : localUnit,
                                                    //         isPlural,
                                                    //         unitLookupTable
                                                    //     );

                                                    //     const cleanedUnit = result.cleaned.trim();

                                                    //     // 4. Update unit state (so UI reflects the corrected plural form)
                                                    //     if (isAddRow && cleanedUnit !== unit) {
                                                    //         setAddRow(prev => ({ ...prev, unit: cleanedUnit }));
                                                    //     }

                                                    //     if (!isAddRow && cleanedUnit !== localUnit) {
                                                    //         setLocalUnit(cleanedUnit);
                                                    //     }
                                                    // }}

                                                    onBlur={(e) => {
                                                        const text = e.target.value;
                                                        //const text = e.target.value.replace(/to/gi, "-");

                                                        // Parse raw input into qty + qtyMax
                                                        const { quantity, quantityMax } = parseQtyInput(text);

                                                        // Clean both sides
                                                        const cleanedQty = trimQuantity(quantity);
                                                        const cleanedQtyMax = trimQuantity(quantityMax);

                                                        // Determine pluralization quantity
                                                        const qtyForPlural = cleanedQtyMax || cleanedQty;

                                                        // Recalculate pluralization using the larger quantity
                                                        const isPlural = requiresPlural(qtyForPlural, measurementSystem);

                                                        // Rebuild the display string (desktop spacing shown here)
                                                        const rebuilt = cleanedQtyMax
                                                            ? `${cleanedQty}-${cleanedQtyMax}`
                                                            : cleanedQty;

                                                        // Update DOM
                                                        //e.target.value = rebuilt;
                                                        setLocalQtyInput(rebuilt);

                                                        // Update ingredient state
                                                        if (isAddRow) {
                                                            // setAddRow(prev => ({
                                                            //     ...prev,
                                                            //     quantity: cleanedQty,
                                                            //     quantityMax: cleanedQtyMax
                                                            // }));
                                                            //setLocalQty(cleanedQty);
                                                            //setLocalQtyMax(cleanedQtyMax);
                                                        } else {
                                                            setLocalQty(cleanedQty);
                                                            setLocalQtyMax(cleanedQtyMax);
                                                        }

                                                        // Now update the unit based on pluralization
                                                        const result = validateUnitInput(
                                                            measurementSystem,
                                                            isAddRow ? unit : localUnit,
                                                            isPlural,
                                                            unitLookupTable
                                                        );

                                                        const cleanedUnit = result.cleaned.trim();

                                                        if (isAddRow) {
                                                            if (cleanedUnit !== unit) {
                                                                setAddRow(prev => ({ ...prev, unit: cleanedUnit }));
                                                            }
                                                        } else {
                                                            if (cleanedUnit !== localUnit) {
                                                                setLocalUnit(cleanedUnit);
                                                            }
                                                        }
                                                    }}


                                                    onChange={(e) => {
                                                        const text = e.target.value;
                                                        //const text = e.target.value.replace(/to/gi, "-");
                                                        setLocalQtyInput(text);

                                                        const { quantity, quantityMax } = parseQtyInput(text);

                                                        if (isAddRow) {
                                                            setAddRow(prev => ({
                                                                ...prev,
                                                                quantity,
                                                                quantityMax
                                                            }));


                                                            // setLocalQty(quantity);
                                                            // setLocalQtyMax(quantityMax);
                                                        } else {
                                                            setLocalQty(quantity);
                                                            setLocalQtyMax(quantityMax);
                                                        }
                                                    }}
                                                />
                                            </div>
                                        </div>

                                        {/* Unit */}
                                        <div className="fixed-textbox unit-input height-65">
                                            <div className="label-mobile">Unit</div>
                                            <div className="form-element">
                                                <input
                                                    type="text"
                                                    className="form-control textbox textbox-small textbox-text"
                                                    maxLength={20}
                                                    value={unit}
                                                    onChange={(e) => {
                                                        if (isAddRow) {
                                                            setAddRow?.(prev => ({ ...prev, unit: e.target.value }));
                                                        } else {
                                                            setLocalUnit(e.target.value);
                                                        }
                                                    }}
                                                    // onBlur={(e) => {
                                                    //     const raw = e.target.value;

                                                    //     // ⭐ Use the DOM qty, not props
                                                    //     const qtyValue = qtyRef.current?.value ?? "";

                                                    //     const isPlural = requiresPlural(qtyValue, measurementSystem);

                                                    //     const result = validateUnitInput(
                                                    //         measurementSystem,
                                                    //         raw,
                                                    //         isPlural,
                                                    //         unitLookupTable
                                                    //     );

                                                    //     const cleaned = result.isValid ? result.cleaned.trim() : raw.trim();

                                                    //     e.target.value = cleaned;

                                                    //     if (isAddRow && cleaned !== unit) {
                                                    //         setAddRow(prev => ({ ...prev, unit: cleaned }));
                                                    //     }

                                                    //     if (!isAddRow && cleaned !== localUnit) {
                                                    //         setLocalUnit(cleaned);
                                                    //     }
                                                    // }}
                                                    onBlur={(e) => {
                                                        const raw = e.target.value;

                                                        // ⭐ Get the raw qty text from the DOM
                                                        const qtyValue = qtyRef.current?.value ?? "";

                                                        // ⭐ Parse the qty input into min/max
                                                        const { quantity, quantityMax } = parseQtyInput(qtyValue);

                                                        // ⭐ Clean both sides
                                                        const cleanedQty = trimQuantity(quantity);
                                                        const cleanedQtyMax = trimQuantity(quantityMax);

                                                        // ⭐ Use the larger quantity for pluralization
                                                        const qtyForPlural = cleanedQtyMax || cleanedQty;

                                                        const isPlural = requiresPlural(qtyForPlural, measurementSystem);

                                                        // ⭐ Validate + autocorrect the unit
                                                        const result = validateUnitInput(
                                                            measurementSystem,
                                                            raw,
                                                            isPlural,
                                                            unitLookupTable
                                                        );

                                                        const cleaned = result.isValid ? result.cleaned.trim() : raw.trim();

                                                        // ⭐ Update DOM
                                                        //e.target.value = cleaned;

                                                        // ⭐ Update state
                                                        if (isAddRow && cleaned !== unit) {
                                                            setAddRow(prev => ({ ...prev, unit: cleaned }));
                                                        }

                                                        if (!isAddRow && cleaned !== localUnit) {
                                                            setLocalUnit(cleaned);
                                                        }
                                                    }}
                                                />
                                                {/* <div className="blur-catcher" tabIndex={0}></div> */}
                                            </div>
                                        </div>

                                        {/* Description */}
                                        <div className="fixed-textbox-large desc-input height-65">
                                            <div className="label-mobile">Description</div>
                                            <span className="required">*</span>
                                            <div className="form-element">
                                                <input
                                                    type="text"
                                                    className="form-control textbox textbox-large textbox-text"
                                                    maxLength={50}
                                                    value={desc}
                                                    onChange={(e) => {
                                                        if (isAddRow) {
                                                            setAddRow?.(prev => ({ ...prev, description: e.target.value }));
                                                        } else {
                                                            setLocalDesc(e.target.value);
                                                        }
                                                    }}
                                                />
                                            </div>
                                        </div>

                                        {/* Instructions */}
                                        <div className="fixed-textbox-large inst-input height-65">
                                            <div className="label-mobile">Preparation (i.e. chopped, ground)</div>
                                            <div className="form-element">
                                                <input
                                                    type="text"
                                                    className="form-control textbox textbox-large textbox-text"
                                                    maxLength={50}
                                                    value={instr}
                                                    onChange={(e) => {
                                                        if (isAddRow) {
                                                            setAddRow?.(prev => ({ ...prev, instructions: e.target.value }));
                                                        } else {
                                                            setLocalInstr(e.target.value);
                                                        }
                                                    }}
                                                />
                                            </div>
                                        </div>

                                        {/* Save + Delete */}
                                        <div className="button-row-tiny d-flex justify-content-end pt-2">
                                            <div className="fixed-button-icon save-input">
                                                {!isAddRow ? (
                                                    <button className="button button-icon-responsive" onClick={onSave}>
                                                        <Icon name="save" marginLeft={3} width={26} height={26} />
                                                    </button>
                                                ) : (
                                                    <button className="button button-icon-responsive" onClick={onAdd}>
                                                        <Icon name="save" marginLeft={3} width={26} height={26} />
                                                    </button>
                                                )}
                                            </div>

                                            <div className="fixed-button-icon del-input ms-2">
                                                {!isAddRow ? (
                                                    <button
                                                        className="button button-icon-responsive button-icon-delete"
                                                        onClick={() => openDeleteModal?.(ingredient!)}
                                                    >
                                                        <Icon name="delete" marginLeft={4} marginTop={-3} width={25} height={25} />
                                                    </button>
                                                ) : (
                                                    <span className="button-icon">&nbsp;</span>
                                                )}
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>

                        </>
                    )}
                </div>
            </>
        );
    }



    return (
        <div
            ref={setNodeRef}
            style={style}
            {...(!isAddRow ? attributes : {})}
            className={`d-flex align-items-start grid-page-row grid-page-row-height-desktop sortable-container ${draggingClass}`}
        >


            {/* LEFT: Drag + Qty + Unit */}
            <div className="d-flex">
                {/* Drag handle */}
                <div className="drag-handle-wrapper">
                    {!isAddRow ? (
                        <div className="drag-handle drag-handle-width-desktop" {...listeners}>
                            <Icon name="drag" />
                        </div>
                    ) : (
                        <div className="drag-handle-width-desktop">&nbsp;</div>
                    )}
                </div>

                {/* Qty */}
                <div className="fixed-textbox">
                    <input
                        ref={qtyRef}
                        type="text"
                        className="form-control textbox textbox-small textbox-text"
                        maxLength={20}
                        value={localQtyInput}

                        // onBlur={(e) => {
                        //     const cleanedQty = trimQuantity(e.target.value);

                        //     // Update DOM
                        //     e.target.value = cleanedQty;

                        //     // 1. Update qty state
                        //     if (isAddRow && cleanedQty !== qty) {
                        //         setAddRow(prev => ({ ...prev, quantity: cleanedQty }));
                        //     }

                        //     if (!isAddRow && cleanedQty !== localQty) {
                        //         setLocalQty(cleanedQty);
                        //     }

                        //     // 2. Recalculate pluralization using the cleaned qty
                        //     const isPlural = requiresPlural(cleanedQty, measurementSystem);

                        //     // 3. Re-validate + autocorrect the unit
                        //     const result = validateUnitInput(
                        //         measurementSystem,
                        //         isAddRow ? unit : localUnit,
                        //         isPlural,
                        //         unitLookupTable
                        //     );

                        //     const cleanedUnit = result.cleaned.trim();

                        //     // 4. Update unit state (so UI reflects the corrected plural form)
                        //     if (isAddRow && cleanedUnit !== unit) {
                        //         setAddRow(prev => ({ ...prev, unit: cleanedUnit }));
                        //     }

                        //     if (!isAddRow && cleanedUnit !== localUnit) {
                        //         setLocalUnit(cleanedUnit);
                        //     }
                        // }}

                        // onChange={(e) => {
                        //     if (isAddRow) {
                        //         setAddRow?.(prev => ({ ...prev, quantity: e.target.value }));
                        //     } else {
                        //         setLocalQty(e.target.value);
                        //     }
                        // }}



                        onBlur={(e) => {
                            const text = e.target.value;
                            //const text = e.target.value.replace(/to/gi, "-");

                            // Parse raw input into qty + qtyMax
                            const { quantity, quantityMax } = parseQtyInput(text);

                            // Clean both sides
                            const cleanedQty = trimQuantity(quantity);
                            const cleanedQtyMax = trimQuantity(quantityMax);

                            // Determine pluralization quantity
                            const qtyForPlural = cleanedQtyMax || cleanedQty;

                            // Recalculate pluralization using the larger quantity
                            const isPlural = requiresPlural(qtyForPlural, measurementSystem);

                            // Rebuild the display string (desktop spacing shown here)
                            const rebuilt = cleanedQtyMax
                                ? `${cleanedQty}-${cleanedQtyMax}`
                                : cleanedQty;

                            // Update DOM
                            //e.target.value = rebuilt;
                            setLocalQtyInput(rebuilt);

                            // Update ingredient state
                            if (isAddRow) {
                                // setAddRow(prev => ({
                                //     ...prev,
                                //     quantity: cleanedQty,
                                //     quantityMax: cleanedQtyMax
                                // }));
                                //setLocalQty(cleanedQty);
                                //setLocalQtyMax(cleanedQtyMax);
                            } else {
                                setLocalQty(cleanedQty);
                                setLocalQtyMax(cleanedQtyMax);
                            }

                            // Now update the unit based on pluralization
                            const result = validateUnitInput(
                                measurementSystem,
                                isAddRow ? unit : localUnit,
                                isPlural,
                                unitLookupTable
                            );

                            const cleanedUnit = result.cleaned.trim();

                            if (isAddRow) {
                                if (cleanedUnit !== unit) {
                                    setAddRow(prev => ({ ...prev, unit: cleanedUnit }));
                                }
                            } else {
                                if (cleanedUnit !== localUnit) {
                                    setLocalUnit(cleanedUnit);
                                }
                            }
                        }}

                        onChange={(e) => {
                            const text = e.target.value;
                            //const text = e.target.value.replace(/to/gi, "-");
                            setLocalQtyInput(text);

                            const { quantity, quantityMax } = parseQtyInput(text);

                            if (isAddRow) {
                                setAddRow(prev => ({
                                    ...prev,
                                    quantity,
                                    quantityMax
                                }));
                                //setLocalQty(quantity);
                                //setLocalQtyMax(quantityMax);
                            } else {
                                setLocalQty(quantity);
                                setLocalQtyMax(quantityMax);
                            }
                        }}

                    />
                </div>

                {/* Unit */}
                <div className="fixed-textbox">
                    <input
                        type="text"
                        className="form-control textbox textbox-small textbox-text"
                        maxLength={20}
                        value={unit}
                        // onBlur={(e) => {
                        //     const raw = e.target.value;

                        //     // ⭐ Use the DOM qty, not props
                        //     const qtyValue = qtyRef.current?.value ?? "";

                        //     const isPlural = requiresPlural(qtyValue, measurementSystem);

                        //     const result = validateUnitInput(
                        //         measurementSystem,
                        //         raw,
                        //         isPlural,
                        //         unitLookupTable
                        //     );

                        //     const cleaned = result.isValid ? result.cleaned.trim() : raw.trim();

                        //     e.target.value = cleaned;

                        //     if (isAddRow && cleaned !== unit) {
                        //         setAddRow(prev => ({ ...prev, unit: cleaned }));
                        //     }

                        //     if (!isAddRow && cleaned !== localUnit) {
                        //         setLocalUnit(cleaned);
                        //     }
                        // }}

                        onBlur={(e) => {
                            const raw = e.target.value;

                            // ⭐ Get the raw qty text from the DOM
                            const qtyValue = qtyRef.current?.value ?? "";

                            // ⭐ Parse the qty input into min/max
                            const { quantity, quantityMax } = parseQtyInput(qtyValue);

                            // ⭐ Clean both sides
                            const cleanedQty = trimQuantity(quantity);
                            const cleanedQtyMax = trimQuantity(quantityMax);

                            // ⭐ Use the larger quantity for pluralization
                            const qtyForPlural = cleanedQtyMax || cleanedQty;

                            const isPlural = requiresPlural(qtyForPlural, measurementSystem);

                            // ⭐ Validate + autocorrect the unit
                            const result = validateUnitInput(
                                measurementSystem,
                                raw,
                                isPlural,
                                unitLookupTable
                            );

                            const cleaned = result.isValid ? result.cleaned.trim() : raw.trim();

                            // ⭐ Update DOM
                            //e.target.value = cleaned;

                            // ⭐ Update state
                            if (isAddRow && cleaned !== unit) {
                                setAddRow(prev => ({ ...prev, unit: cleaned }));
                            }

                            if (!isAddRow && cleaned !== localUnit) {
                                setLocalUnit(cleaned);
                            }
                        }}


                        onChange={(e) => {
                            if (isAddRow) {
                                setAddRow?.(prev => ({ ...prev, unit: e.target.value }));
                            } else {
                                setLocalUnit(e.target.value);
                            }
                        }}

                    />
                </div>
            </div>

            {/* MIDDLE: Description + Instructions */}
            <div className="flex-grow-1">
                <div className="row">

                    {!isAddRow && (
                        <>
                            <input type="hidden" value={ingredient!.id} />
                            <input type="hidden" value={ingredient!.sortOrder ?? index + 1} />
                        </>
                    )}

                    {/* Description (col-7) */}
                    <div className="col-7">
                        <input
                            type="text"
                            className="form-control textbox textbox-large textbox-text"
                            maxLength={50}
                            value={desc}
                            onChange={(e) => {
                                if (isAddRow) {
                                    setAddRow?.(prev => ({ ...prev, description: e.target.value }));
                                } else {
                                    setLocalDesc(e.target.value);
                                }
                            }}

                        />
                    </div>

                    {/* Instructions (col-5) */}
                    <div className="col-5">
                        <input
                            type="text"
                            className="form-control textbox textbox-large textbox-text"
                            maxLength={50}
                            value={instr}
                            onChange={(e) => {
                                if (isAddRow) {
                                    setAddRow?.(prev => ({ ...prev, instructions: e.target.value }));
                                } else {
                                    setLocalInstr(e.target.value);
                                }
                            }}

                        />
                    </div>

                    {/* Qty column placeholder (col-1) */}
                    {/* <div className="col-1">&nbsp;</div> */}
                </div>
            </div>

            {/* RIGHT: Save + Delete */}
            <div className="d-flex ps-3">

                {/* Save / Add */}
                <div className="fixed-button-icon">
                    {!isAddRow ? (
                        <button className="button button-icon" onClick={onSave}>
                            <Icon name="save" width={23} height={23} />
                        </button>
                    ) : (
                        <button className="button button-icon" onClick={onAdd}>
                            <Icon name="add" marginLeft={1} />
                        </button>

                    )}
                </div>

                {/* Delete */}
                <div className="fixed-button-icon">
                    {!isAddRow ? (
                        <button
                            className="button button-icon button-icon-delete"
                            onClick={() => openDeleteModal?.(ingredient!)}
                        >
                            <Icon name="delete" marginLeft={2} marginTop={-5} width={21} height={21} />
                        </button>
                    ) : (
                        <span className="button-icon">&nbsp;</span>
                    )}
                </div>

            </div>
        </div>

    );
};

export default SortableIngredientItem;
