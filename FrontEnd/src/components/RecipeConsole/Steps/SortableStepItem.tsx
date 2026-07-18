import React, { useState, useEffect, useRef } from "react";
import { useSortable } from "@dnd-kit/sortable";
import { useDndContext } from '@dnd-kit/core';
import { CSS } from "@dnd-kit/utilities";
import Icon from "../../UserControls/Icons/icons";
import type { Step } from "../../../types/Recipe/Recipe";
import type { MeasurementUnit } from "src/types/Measurement/MeasurementType";
import { ContentEditor } from '../../UserControls/ContentEditor/ContentEditor';
import { renderStep } from '../../../helpers/measurementHelper';

export interface ContenteditorHandle {
    isEditorEmpty: () => boolean;
    // you MUST add this:
    focus: () => void;
}





interface Props {
    step: Step | null;
    index: number;

    // Normal row
    handleSave?: (step: Step) => void;
    openDeleteModal?: () => void;

    // Add row
    isAddRow?: boolean;
    handleAdd?: (added: Step) => void;

    deviceType: "desktop" | "mobile";

    recentlySavedId: number | null;
    setAddRow?: React.Dispatch<React.SetStateAction<Step>>;

    // NEW: validation + pending action
    openValidationModal: (
        errors: string[],
        action: "add" | "save",
        step: Step
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


const SortableStepItem: React.FC<Props> = ({
    step,
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
    // For Add Row, derive values from parent-controlled step.
    //const [localQty, setLocalQty] = useState(step?.quantity ?? "");
    //const [localUnit, setLocalUnit] = useState(step?.unit ?? "");
    const [localDesc, setLocalDesc] = useState(step?.description ?? "");
    //const [localInstr, setLocalInstr] = useState(step?.instructions ?? "");

    // Unified values used by the UI
    //const qty = isAddRow ? step?.quantity ?? "" : localQty;
    //const unit = isAddRow ? step?.unit ?? "" : localUnit;
    const desc = isAddRow ? step?.description ?? "" : localDesc;
    //const instr = isAddRow ? step?.instructions ?? "" : localInstr;

    const [frozenInitialHtml] = useState(step?.description ?? "");



    const descRef = useRef<ContenteditorHandle>(null);



    const isHighlighted = !isAddRow && step?.id === recentlySavedId;

    const sortable = !isAddRow
        ? useSortable({ id: step!.id.toString() })
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
        //const cleanedQty = trimQuantity(qty);

        // 2. Plural logic
        // const isPlural = requiresPlural(cleanedQty, measurementSystem);

        // 3. Validate + clean unit
        // const result = validateUnitInput(
        //     measurementSystem,
        //     unit,
        //     isPlural,
        //     unitLookupTable
        // );
        // const cleanedUnit = result.cleaned.trim();

        // 4. Clean description + instructions
        const cleanedDesc = desc.trim();
        //const cleanedInstr = instr.trim();

        // 5. Update local state so UI reflects cleaned values
        //setLocalQty(cleanedQty);
        //setLocalUnit(cleanedUnit);
        setLocalDesc(cleanedDesc);
        //setLocalInstr(cleanedInstr);

        // 6. Perform save with cleaned values
        handleSave?.({
            ...step!,
            description: cleanedDesc
        });
    };



    const onAdd = () => {
        setAddRow(prev => {
            // const cleanedQty = trimQuantity(prev.quantity);
            // const isPlural = requiresPlural(cleanedQty, measurementSystem);

            // const result = validateUnitInput(
            //     measurementSystem,
            //     prev.unit,
            //     isPlural,
            //     unitLookupTable
            // );

            const cleanedStep = {
                ...prev,
                description: prev.description.trim(),
                id: 0,
                sortOrder: index + 1,
                isActive: true
            };

            handleAdd?.(cleanedStep);

            return cleanedStep;
        });
    };


    useEffect(() => {
        if (isAddRow) {
            const isBlank =
                //(step?.quantity ?? "") === "" &&
                //(step?.unit ?? "") === "" &&
                (step?.description ?? "") === ""
            //(step?.instructions ?? "") === "";

            if (isBlank) {
                setTimeout(() => {
                    //console.log("ADD descRef.current:", descRef.current);

                    descRef.current?.focus();
                }, 50);

            }
        }
    }, [
        isAddRow,
        //step?.quantity,
        //step?.unit,
        step?.description,
        //step?.instructions
    ]);


    useEffect(() => {

        if (!isAddRow && step?.id === recentlySavedId) {
            setTimeout(() => {
                console.log("UPDATE descRef.current:", descRef.current);

                descRef.current?.focus();
            }, 50);

        }
    }, [recentlySavedId, isAddRow, step]);

    // useEffect(() => {
    //     if (!isAddRow && editor && step?.description !== frozenInitialHtml) {
    //         editor.commands.setContent(step.description);
    //     }
    // }, [step?.description]);



    //if (deviceType === "mobile") {
    const { active } = useDndContext();
    const isDragging = active?.id === step.id.toString();

    const expandedClass = isOpen ? "expanded-content-tint" : "";
    const draggingClass = isDragging ? "drag-item-tint" : "";

    return (
        <>
            <div
                ref={setNodeRef}
                style={style}
                className={`mobile-step-row grid-page-row item-tint ${expandedClass} ${draggingClass}`}

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
                                <span className="fw-bold">Add Step</span>
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


                                    {/* Description */}
                                    <div className="col-8 ps-3 ">
                                        <span
                                            className="truncate-one-line"
                                            dangerouslySetInnerHTML={{ __html: renderStep(desc, "SansSerif") }}
                                        />

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
                        <div className="pt-1 accordion-content expanded-content" data-expand-id={isAddRow ? "ADD_ROW" : step.id}>
                            <div className="p-2 d-flex flex-wrap accordion">
                                <div className="accordion-content-inner ps-1 pe-1">

                                    {/* Description */}
                                    <div className="fixed-textbox-large desc-input height-65">
                                        {/* <div className="label-mobile">Description</div>
                                        <span className="required">*</span> */}
                                        <div className="form-element">
                                            <div className="edit-section-content">
                                                <ContentEditor
                                                    ref={descRef}
                                                    editorId={`step-${step.id}`}
                                                    initialHtml={frozenInitialHtml}
                                                    onChangeHtml={(html) => {
                                                        if (isAddRow) {
                                                            setAddRow?.(prev => ({ ...prev, description: html }))
                                                        } else {
                                                            setLocalDesc(html)
                                                        }
                                                    }}
                                                />
                                            </div>



                                        </div>
                                    </div>


                                    {/* Save + Delete */}
                                    <div className="button-row-tiny d-flex justify-content-end pt-2">
                                        <div className="fixed-button-icon save-input">
                                            {!isAddRow ? (
                                                <button className="button button-icon" onClick={onSave}>
                                                    {deviceType === "desktop" ? (
                                                        <div className="margin-2"><Icon name="save" width={23} height={23} /></div>
                                                    ) : (
                                                        <div className="margin-3"><Icon name="save" width={26} height={26} /></div>
                                                    )}
                                                </button>
                                            ) : (
                                                <button className="button button-icon" onClick={onAdd}>
                                                    {deviceType === "desktop" ? (
                                                        <div className="margin-2"><Icon name="save" width={23} height={23} /></div>
                                                    ) : (
                                                        <div className="margin-3"><Icon name="save" width={26} height={26} /></div>
                                                    )}
                                                </button>
                                            )}
                                        </div>

                                        <div className="fixed-button-icon del-input ms-2">
                                            {!isAddRow ? (
                                                <button
                                                    className="button button-icon button-icon-delete"
                                                    onClick={() => openDeleteModal?.(step!)}
                                                >
                                                    {deviceType === "desktop" ? (
                                                        <div className="margin-4" style={{ marginTop: -3 }}>
                                                            <div style={{ marginTop: -5, marginLeft: 2 }}><Icon name="delete" width={21} height={21} /></div>
                                                        </div>
                                                    ) : (
                                                        <div className="margin-4" style={{ marginTop: -3 }}>
                                                            <Icon name="delete" width={25} height={25} />
                                                        </div>
                                                    )}

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
//};

export default SortableStepItem;
