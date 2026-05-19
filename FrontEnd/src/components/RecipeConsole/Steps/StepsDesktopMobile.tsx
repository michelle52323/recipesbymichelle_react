import React, { useState, useEffect } from "react";
import Modal from "react-modal";
import {
    DndContext,
    closestCenter,
    useSensor,
    useSensors,
    PointerSensor,
    KeyboardSensor,
    TouchSensor
} from '@dnd-kit/core';
import {
    arrayMove,
    SortableContext,
    useSortable,
    sortableKeyboardCoordinates,
    verticalListSortingStrategy,
} from '@dnd-kit/sortable';
import Loader from "../../UserControls/Loader/Loader";
import SortableStepItem from "./SortableStepItem";
import type { StepGridController } from "../../../types/Recipe/StepsGrid";



Modal.setAppElement("#root");

interface Props {
    controller: StepGridController;
}

function StepsListMobile({ controller }: Props) {
    const {
        grid,
        modalIsOpen,
        handleDragEnd,
        handleDelete,
        handleSave,
        handleAdd,
        openDeleteModal,
        stepToDelete,
        stepIndexToDelete,
        isLoading
    } = controller;

    const steps = grid.steps;

    //const scrollRef = React.useRef<HTMLDivElement>(null);

    const sensors = useSensors(
        useSensor(PointerSensor),
        useSensor(TouchSensor),
        useSensor(KeyboardSensor, {
            coordinateGetter: sortableKeyboardCoordinates,
        })
    );


    useEffect(() => {
        if (controller.scrollBoxRef.current) {
            controller.scrollBoxRef.current.scrollTop = controller.scrollBoxRef.current.scrollHeight;
        }
    }, [steps.length]);

    if (isLoading) {
        return <Loader message="Loading steps ..." />;
    }

    return (
        <div className="pt-3">
            <div className="content-inner-desktop">

                <DndContext
                    sensors={sensors}
                    collisionDetection={closestCenter}
                    onDragEnd={handleDragEnd}
                >
                    <SortableContext
                        items={steps.map(i => i.id.toString())}
                        strategy={verticalListSortingStrategy}
                    >
                        <div className="d-flex align-items-start mobile-collapsed-row header-row">

                            {/* Drag handle column */}
                            <div className="drag-handle-column">
                                <div className="mobile-drag-handle-wrapper">
                                    &nbsp;
                                </div>
                            </div>

                            {/* Content column */}
                            <div className="content-column flex-grow-1">
                                <div className="row ">

                                    {/* Quantity label */}
                                    <div className="col-4">
                                        <div className="d-flex align-items-baseline flex-wrap">
                                            <span className="mobile-header-label"><b>Quantity</b></span>
                                        </div>
                                    </div>

                                    {/* Description label */}
                                    <div className="col-8 ps-3">
                                        <span className="mobile-header-label"><b>Description</b></span>
                                    </div>
                                </div>
                            </div>

                            {/* Action column */}
                            <div className="action-column">
                                <div className="fixed-button">
                                    &nbsp;
                                </div>
                            </div>
                        </div>



                        {/* Rows */}
                        <div className="grid-overflow-box gof-editable-mobile-short" id="sortable" ref={controller.scrollBoxRef}>
                            <div id="steps-list-container">
                                {steps.map((step, i) => (
                                    <SortableStepItem
                                        key={step.id}
                                        step={step}
                                        index={i}
                                        handleSave={handleSave}
                                        isAddRow={false}
                                        handleAdd={handleAdd}
                                        openDeleteModal={() =>
                                            openDeleteModal(step, i + 1)
                                        }
                                        deviceType="mobile"
                                        recentlySavedId={controller.recentlySavedId}
                                        openValidationModal={controller.openValidationModal}
                                        pendingAction={controller.pendingAction}
                                        setPendingAction={controller.setPendingAction}
                                        unitLookupTable={controller.grid.unitLookupTable}
                                        measurementSystem={controller.measurementSystem}
                                        onToggle={() => controller.onToggle(step.id.toString())}

                                        isOpen={controller.openId === step.id.toString()}

                                    />
                                ))}
                            </div>
                            <div id="steps-list-container">

                                <SortableStepItem
                                    step={controller.addRow}
                                    index={steps.length}
                                    isAddRow={true}
                                    handleAdd={handleAdd}
                                    deviceType="mobile"
                                    setAddRow={controller.setAddRow}
                                    openValidationModal={controller.openValidationModal}
                                    pendingAction={controller.pendingAction}
                                    setPendingAction={controller.setPendingAction}
                                    unitLookupTable={controller.grid.unitLookupTable}
                                    measurementSystem={controller.measurementSystem}
                                    onToggle={() => controller.onToggle(controller.ADD_ID)}

                                    isOpen={controller.openId === controller.ADD_ID}

                                />

                            </div>
                        </div>
                    </SortableContext>
                </DndContext>
            </div>


        </div>
    );
}

export default StepsListMobile;
