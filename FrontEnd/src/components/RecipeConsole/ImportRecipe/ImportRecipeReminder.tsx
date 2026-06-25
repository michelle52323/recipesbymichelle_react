import React from 'react';
import './importRecipeReminder.css';

import Icon  from '../../UserControls/Icons/icons';


interface ImportRecipeReminderProps {
    onClose?: () => void;
}

function ImportRecipeReminder({ onClose }: ImportRecipeReminderProps) {

    const message = (
        <>
            <strong>Double‑check your imported recipe.</strong><br />
            Every site formats recipes differently, so a few details may need correction.
            Review the info, ingredients, and steps before finalizing.
        </>
    );

    return (
        <div className="import-recipe-reminder">
            <div className="reminder-icon">
                <Icon name="info" />

            </div>

            <div className="reminder-message">
                {message}
            </div>

            {onClose && (
                <button
                    type="button"
                    className="reminder-close"
                    onClick={onClose}
                    aria-label="Close reminder"
                >
                    ×
                </button>
            )}
        </div>
    );
}

export default ImportRecipeReminder;
