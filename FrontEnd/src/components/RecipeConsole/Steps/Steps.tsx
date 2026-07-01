import { useEffect, useState, useRef, useCallback } from "react";
import { useNavigate, useLocation, useOutletContext, useParams } from "react-router-dom";
import Modal from "react-modal";

import { isMobileTouchDevice, getApiBaseUrl } from "../../../helpers/config";

import CheckAuth from "../../../components/Account/CheckAuth";
import { trimQuantity, validateQuantityNumeric, validateUnitInput, requiresPlural } from "../../../helpers/measurementHelper";

import type { LayoutContext } from "../../Layout";

import StepsListDesktopMobile from "./StepsDesktopMobile";

import ProgressBar from "../../UserControls/ProgressBar/ProgressBar";

import ButtonGrid from "../../UserControls/ButtonGrid/ButtonGrid";
import Icon from "../../UserControls/Icons/icons";
import Loader from "../../UserControls/Loader/Loader";
import FavoritesStar from '../../../components/UserControls/Favorites/FavoriteStar';

import type { Step, StepAdd, Unit } from "../../../types/Recipe/Recipe";
import type { FractionDecimal, MeasurementUnit } from "src/types/Measurement/MeasurementType";
import type {
    StepGrid,
    StepGridController,
} from "../../../types/Recipe/StepsGrid";
import type { DragEndEvent } from "@dnd-kit/core";

import { isTipTapEmpty } from '../../UserControls/ContentEditor/UtilityFunctions';

import '../../../grid-layout.css';

//consider removing
import './steps.css';


const API_BASE = getApiBaseUrl();

interface RecipeForm {
    name: string;
    description: string;
}

interface AuthResult {
    auth: boolean;
    claims: Record<string, string>;
}

function Steps() {
    const navigate = useNavigate();
    const location = useLocation();

    const { setTitle } = useOutletContext<{ setTitle: (title: string) => void }>();
    const { setBanner } = useOutletContext<{ setBanner: (message: string) => void }>();
    const { setTitleBarSlot } = useOutletContext<LayoutContext>();

    const { id: recipeId } = useParams();
    if (!recipeId) navigate("/dashboard");

    const [auth, setAuth] = useState<AuthResult | null>(null);

    const [recipe, setRecipe] = useState<RecipeForm>({
        name: "",
        description: "",
    });

    // GRID STATE
    const [steps, setSteps] = useState<Step[]>([]);
    const [validUnits, setValidUnits] = useState<Unit[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [recentlySavedId, setRecentlySavedId] = useState<number | null>(null);

    //ADD ROW STATE
    const [addRow, setAddRow] = useState<Step>({
        id: 0,
        quantity: "",
        unit: "",
        description: "",
        instructions: "",
        sortOrder: null,
        isActive: true
    });



    // DELETE MODAL STATE
    const [modalIsOpen, setModalIsOpen] = useState(false);
    const [stepToDelete, setStepToDelete] = useState<Step | null>(null);
    const [stepIndexToDelete, setStepIndexToDelete] = useState<number | null>(null);

    //VALIDATION MODAL STATE
    // Validation modal state
    const [validationModalIsOpen, setValidationModalIsOpen] = useState(false);
    const [validationErrors, setValidationErrors] = useState<string[]>([]);
    const [pendingAction, setPendingAction] = useState<"add" | "save" | null>(null);


    //MEASUREMENT STATES
    const [measurementSystem, setMeasurementSystem] = useState<"Imperial" | "Metric" | null>(null);
    const [fractionDecimalLookupTable, setFractionDecimalLookupTable] =
        useState<FractionDecimal[]>([]);

    const [unitLookupTable, setUnitLookupTable] =
        useState<MeasurementUnit[]>([]);


    //Mobile expandable open state
    const [openId, setOpenId] = useState<string | null>(null);
    const ADD_ID = "ADD_ROW";

    //Mobile expandable scroll state
    const scrollBoxRef = useRef<HTMLDivElement>(null);

    // -----------------------------
    // AUTH + TITLE + BANNER LOGIC
    // -----------------------------

    useEffect(() => {
        return () => setBanner("");
    }, []);

    useEffect(() => {
        return () => {
            setTitleBarSlot(null);
        };
    }, [setTitleBarSlot]);

    useEffect(() => {
        setTitleBarSlot(<FavoritesStar recipeId={recipeId} starLocation="title" />);
    }, [recipeId]);

    useEffect(() => {
        if (location.state?.banner) {
            setBanner(location.state.banner);
            navigate(location.pathname, { replace: true, state: {} });
        }
    }, [location.state?.banner, navigate, location.pathname, setBanner]);

    useEffect(() => {
        async function hydrateAuth() {
            const result = await CheckAuth();
            if (result && typeof result.auth === "boolean") {
                setAuth(result as AuthResult);
            } else {
                setAuth({ auth: false, claims: {} });
            }
        }
        hydrateAuth();
    }, []);

    useEffect(() => {
        if (auth === null) return;
        if (!auth.auth) navigate("/signin");
        else setTitle(recipe.name);
    }, [auth, navigate, recipe.name, setTitle]);

    //Set Meausrement System
    useEffect(() => {
        if (auth) {

            const claim = auth?.claims?.MeasurementSystem;

            const normalized =
                claim === "Imperial" || claim === "Metric"
                    ? claim
                    : "Imperial";

            setMeasurementSystem(normalized);
            //console.log("Measurement System: " + measurementSystem);
        }
    }, [auth]);


    // -----------------------------
    // RECIPE INFO
    // -----------------------------

    useEffect(() => {
        if (!recipeId) return;

        fetch(`${API_BASE}/api/RecipeInfo/${recipeId}`, { credentials: "include" })
            .then((res) => (res.ok ? res.json() : Promise.reject("Recipe not found")))
            .then((data) => {
                setRecipe({
                    name: data.name,
                    description: data.description,
                });
            })
            .catch(() => navigate("/dashboard"));
    }, [recipeId, navigate]);

    // -----------------------------
    // STEP GRID FETCH
    // -----------------------------

    const loadSteps = useCallback(async () => {
        setIsLoading(true);

        const response = await fetch(
            `${API_BASE}/api/Steps/steps/${recipeId}/`,
            { credentials: "include" }
        );

        if (response.ok) {
            const data = await response.json();
            setSteps(data.steps ?? data); // supports old + new API
            setValidUnits(data.validUnits ?? []);
        } else {
            console.error("Failed to fetch steps");
        }

        setIsLoading(false);
    }, [recipeId]);

    useEffect(() => {
        if (auth?.auth) loadSteps();
    }, [auth, loadSteps]);

    // -----------------------------
    // ADD NEW SAVE HANDLING
    // -----------------------------

    const handleAdd = async (added: Step) => {
        setBanner('');

        // 1. Trim fields
        //const cleanedQty = trimQuantity(added.quantity);
        //const cleanedUnit = added.unit?.trim() ?? "";
        const cleanedDesc = isTipTapEmpty(added.description?.trim()) ? "" : added.description?.trim();
        //const cleanedInstr = added.instructions?.trim() ?? "";



        // 2. Numeric validation (Imperial/Metric)
        const errors: string[] = [];
        //const qtyResult = validateQuantityNumeric(measurementSystem, cleanedQty);
        // if (!qtyResult.isValid) {
        //     errors.push(qtyResult.error!);
        // }

        // 3. Unit validation 
        //const isPlural = requiresPlural(cleanedQty, measurementSystem);
        //const unitResult = validateUnitInput(measurementSystem, cleanedUnit, isPlural, unitLookupTable);
        //if (!unitResult.isValid) errors.push(unitResult.error);

        // 4. Basic blank validation
        if (!cleanedDesc) errors.push("Description is required.");

        // 5. If any errors → open modal and stop
        if (errors.length > 0) {
            setPendingAction("add");

            openValidationModal(
                errors,
                "add",
                {
                    ...added,
                    description: cleanedDesc,
                    sortOrder: added.sortOrder ?? index + 1,
                    isActive: true
                }
            );

            return;
        }

        // 6. Build StepAdd object
        const stepToAdd: StepAdd = {
            ...added,
            //quantity: cleanedQty,
            //unit: cleanedUnit,
            description: cleanedDesc,
            //instructions: cleanedInstr,
            recipeId: Number(recipeId)
        };

        try {
            // 7. Call API
            const response = await fetch(`${API_BASE}/api/Steps`, {
                method: "POST",
                credentials: "include",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(stepToAdd)
            });

            if (!response.ok) throw new Error("Save failed");

            const result = await response.json();

            if (!result.success) {
                throw new Error(result.message ?? "Save failed");
            }

            // Extract the step returned by the API
            const saved: Step = result.step;

            // 8. Add the new step to the list
            setSteps(prev => [...prev, saved]);

            // 9. Reset Add Row
            setAddRow({
                id: 0,
                description: "",
                sortOrder: null,
                isActive: true
            });

            //10.  Close add row (gets applied to mobile only)
            setOpenId(null);

            // 11. Banner
            setBanner('Step successfully added!');

        } catch (err) {
            console.error(err);
            setBanner('Error adding step.');
        }
    };



    // -----------------------------
    // SAVE (UPDATED) HANDLING
    // -----------------------------
    const handleSave = async (updated: Step) => {
        setBanner('');

        // 1. Trim fields
        //const cleanedQty = trimQuantity(updated.quantity);
        //const cleanedUnit = updated.unit?.trim() ?? "";
        const cleanedDesc = isTipTapEmpty(updated.description?.trim()) ? "" : updated.description?.trim();


        // 2. Numeric validation (Imperial/Metric)
        const errors: string[] = [];

        //const qtyResult = validateQuantityNumeric(measurementSystem, cleanedQty);
        // if (!qtyResult.isValid) {
        //     errors.push(qtyResult.error!);
        // }

        // 3. Unit validation 
        // const isPlural = requiresPlural(cleanedQty, measurementSystem);
        // const unitResult = validateUnitInput(measurementSystem, cleanedUnit, isPlural, unitLookupTable);
        // if (!unitResult.isValid) errors.push(unitResult.error);

        // 4. Basic blank validation
        if (!cleanedDesc) errors.push("Description is required.");

        // 5. If any errors → open modal and stop
        if (errors.length > 0) {
            setPendingAction("save");

            openValidationModal(
                errors,
                "save",
                {
                    ...updated,
                    description: cleanedDesc
                }
            );

            return;
        }

        // 6. If valid → perform save
        try {
            const response = await fetch(`${API_BASE}/api/Steps`, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    ...updated,
                    description: cleanedDesc
                })
            });

            if (!response.ok) throw new Error("Save failed");

            setSteps(prev =>
                prev.map(i => (i.id === updated.id ? {
                    ...updated,
                    description: cleanedDesc
                } : i))
            );

            setRecentlySavedId(updated.id);
            setBanner("Step successfully updated!");

        } catch (err) {
            console.error(err);
            setBanner("Error updating step.");
        }
    };

    useEffect(() => {

        controller.recentlySavedId = recentlySavedId;

    }, [recentlySavedId]);




    // -----------------------------
    // DELETE HANDLING
    // -----------------------------

    const openDeleteModal = (step: Step | null, index: number | null) => {
        setStepToDelete(step);
        setStepIndexToDelete(index);
        setModalIsOpen(!!step);
    };

    const handleDelete = async () => {
        if (!stepToDelete) return;

        setBanner("");

        const response = await fetch(`${API_BASE}/api/Steps/${stepToDelete.id}`, {
            method: "DELETE",
            credentials: "include",
        });

        if (response.ok) {
            await loadSteps();
            setBanner("Step successfully deleted!");
        } else {
            setBanner("Error occurred during deletion");
        }

        setModalIsOpen(false);
    };

    // -----------------------------
    // SORT HANDLING
    // -----------------------------

    const handleDragEnd = async (event: any) => {
        setBanner("");

        const { active, over } = event;
        if (!over || active.id === over.id) return;

        const oldIndex = steps.findIndex((i) => i.id.toString() === active.id);
        const newIndex = steps.findIndex((i) => i.id.toString() === over.id);

        const reordered = [...steps];
        const [moved] = reordered.splice(oldIndex, 1);
        reordered.splice(newIndex, 0, moved);

        const updated = reordered.map((i, idx) => ({
            ...i,
            sortOrder: idx + 1,
        }));

        setSteps(updated);

        const response = await fetch(
            `${API_BASE}/api/Steps/update-sort-order`,
            {
                method: "POST",
                credentials: "include",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(
                    updated.map((i) => ({
                        id: i.id,
                        sortOrder: i.sortOrder,
                    }))
                ),
            }
        );

        const result = await response.json();

        if (response.ok && result.success) {
            setBanner("Steps successfully re-ordered!");
        } else {
            setBanner("Error occurred during sorting");
        }
    };

    // -----------------------------
    // VALIDATION HANDLING
    // -----------------------------

    const openValidationModal = (errors: string[]) => {
        setValidationErrors(errors);
        setValidationModalIsOpen(true);
    };

    const closeValidationModal = () => {
        setValidationModalIsOpen(false);
        setValidationErrors([]);
    };

    // -----------------------------
    // MEASUREMENT
    // -----------------------------
    useEffect(() => {
        async function loadFractionLookup() {
            try {
                const response = await fetch(
                    `${API_BASE}/api/Measurement/fractions`,
                    {
                        method: "GET",
                        credentials: "include" // optional but safe
                    }
                );

                if (!response.ok) {
                    console.error("Failed to load fraction lookup table");
                    return;
                }

                const data: FractionDecimal[] = await response.json();
                setFractionDecimalLookupTable(data);
            } catch (err) {
                console.error("Error loading fraction lookup table:", err);
            }
        }

        loadFractionLookup();
    }, []);

    useEffect(() => {
        async function loadUnitLookup() {
            try {
                const response = await fetch(
                    `${API_BASE}/api/Measurement/units`,
                    {
                        method: "GET",
                        credentials: "include" // REQUIRED
                    }
                );

                if (!response.ok) {
                    console.error("Failed to load unit lookup table");
                    return;
                }

                const data: MeasurementUnit[] = await response.json();
                setUnitLookupTable(data);
            } catch (err) {
                console.error("Error loading unit lookup table:", err);
            }
        }

        loadUnitLookup();
    }, []);

    // useEffect(() => {
    //     if (fractionDecimalLookupTable.length > 0) {
    //         console.log(
    //             "Fraction Decimal Lookup Table:",
    //             JSON.stringify(fractionDecimalLookupTable, null, 2)
    //         );
    //     }
    // }, [fractionDecimalLookupTable]);

    // useEffect(() => {
    //     if (unitLookupTable.length > 0) {
    //         console.log(
    //             "Unit Lookup Table:",
    //             JSON.stringify(unitLookupTable, null, 2)
    //         );
    //     }
    // }, [unitLookupTable]);

    const onToggle = (id: string) => {
        setOpenId(prev => (prev === id ? null : id));
    };

    //ORGINAL SCROLL BEHAVIOR FOR EXPANDABLE
    // useEffect(() => {
    //     if (!openId) return;

    //     const scrollBox = controller.scrollBoxRef.current;
    //     if (!scrollBox) return;

    //     const expanded = scrollBox.querySelector(
    //         `[data-expand-id="${openId}"]`
    //     );
    //     if (!expanded) return;

    //     (expanded as HTMLElement).scrollIntoView({
    //         behavior: "smooth",
    //         block: "nearest"
    //     });
    // }, [openId]);

    useEffect(() => {
        if (!openId) return;

        const scrollBox = scrollBoxRef.current;
        if (!scrollBox) return;

        const expanded = scrollBox.querySelector(
            `[data-expand-id="${openId}"]`
        ) as HTMLElement | null;

        if (!expanded) return;

        const boxRect = scrollBox.getBoundingClientRect();
        const elRect = expanded.getBoundingClientRect();

        // distance from top of scrollBox to element, plus current scroll
        const offset = elRect.top - boxRect.top + scrollBox.scrollTop - 48;

        scrollBox.scrollTo({
            top: offset,
            behavior: "smooth",
        });
    }, [openId]);



    // -----------------------------
    // CONTROLLER OBJECT
    // -----------------------------

    const controller: StepGridController = {
        grid: {
            steps,
            unitLookupTable,
        },
        modalIsOpen,
        stepToDelete,
        stepIndexToDelete,
        isLoading,
        handleDragEnd,
        handleDelete,
        handleSave,
        handleAdd,
        openDeleteModal,
        recentlySavedId,
        addRow,
        setAddRow,
        measurementSystem,
        validationModalIsOpen,
        validationErrors,
        openValidationModal,
        closeValidationModal,
        pendingAction,
        setPendingAction,
        openId,
        setOpenId,
        onToggle,
        ADD_ID,
        scrollBoxRef
    };

    // -----------------------------
    // RENDER
    // -----------------------------

    if (auth === null) return <Loader message="Loading steps ..." />;
    if (!auth.auth) return null;

    return (
        <>
            <div className="page-container w-100">
                <div
                    className={
                        isMobileTouchDevice()
                            ? "content-holder-mobile"
                            : "content-holder-desktop"
                    }
                >
                    <ProgressBar />

                    {isMobileTouchDevice() ? (
                        <StepsListDesktopMobile controller={controller} />
                    ) : (
                        <StepsListDesktopMobile controller={controller} />
                    )}
                </div>
            </div>

            <ButtonGrid
                buttons={[
                    {
                        text: "Back",
                        url: `/recipes/ingredients/${recipeId}`,
                        icon: <Icon name="leftArrow" />,
                        type: "button",
                        mobileSlot: 1,
                        desktopSlot: 1,
                    },
                    {
                        text: "My Recipes",
                        url: "/recipes/myrecipes",
                        type: "button",
                        mobileSlot: 2,
                        desktopSlot: 3,
                    },
                    {
                        text: "Complete",
                        url: `/recipes/view/${recipeId}`,
                        type: "button",
                        mobileSlot: 3,
                        desktopSlot: 5,
                    }
                ]}
            />

            {/* DELETE MODAL */}
            <Modal
                isOpen={modalIsOpen}
                onRequestClose={() => openDeleteModal(null, null)}
                contentLabel="Confirm Delete"
                className="dialog-wrapper"
            >
                <div className="modal-header dialog-header">
                    <h5 className="modal-title">Confirm Delete</h5>
                    <button
                        className="btn-close"
                        onClick={() => openDeleteModal(null, null)}
                    ></button>
                </div>

                <div className="dialog-content-holder">
                    <div className="dialog-content modal-body dialog-text">
                        <div>
                            Are you sure you want to delete step?
                        </div>

                        <div
                            className="mt-2"
                            dangerouslySetInnerHTML={{
                                __html: stepToDelete?.description ?? "",
                            }}
                        />

                    </div>

                    <div className="dialog-footer d-flex justify-content-end gap-2">
                        <button
                            className="button button-modal"
                            onClick={() => openDeleteModal(null, null)}
                        >
                            Cancel
                        </button>

                        <button
                            className="button button-modal"
                            onClick={handleDelete}
                        >
                            Yes, Delete
                        </button>
                    </div>
                </div>
            </Modal>

            {/* VALIDATION MODAL */}
            <Modal
                isOpen={controller.validationModalIsOpen}
                onRequestClose={controller.closeValidationModal}
                contentLabel="Validation Errors"
                className="dialog-wrapper"
            >
                <div className="modal-header dialog-header">
                    <h5 className="modal-title">Please fix the following</h5>
                    <button className="btn-close" onClick={controller.closeValidationModal}></button>
                </div>

                <div className="dialog-content-holder">
                    <div className="dialog-content modal-body dialog-text">
                        {controller.validationErrors.map((err, idx) => (
                            <div key={idx}>{err}</div>
                        ))}
                    </div>

                    <div className="dialog-footer d-flex justify-content-end gap-2">
                        <button className="button button-modal" onClick={controller.closeValidationModal}>
                            OK
                        </button>
                    </div>
                </div>
            </Modal>

        </>
    );
}

export default Steps;
