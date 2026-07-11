import Icon from '../Icons/icons'   // adjust path as needed
import './textboxunique.css';

interface TextboxUniqueProps {
    value: string;
    onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
    onBlur?: () => void;
    name?: string;
    placeholder?: string;
    className?: string;
    status: 'none' | 'available' | 'taken';
    deviceType?: "desktop" | "mobile";
    disabled?: boolean | null;
}

export default function TextboxUnique({
    value,
    onChange,
    onBlur,
    name = '',
    placeholder = '',
    className = 'form-control textbox textbox-text textbox-large',
    status,
    deviceType = "desktop",
    disabled = false
}: TextboxUniqueProps) {
    const successIconSize = deviceType == "desktop" ? 21.5 : 25;
    const errorIconSize = deviceType == "desktop" ? 21.5 : 25;
    return (
        <div className="form-element validated-input-wrapper" style={{ position: 'relative' }}>
            <input
                type="text"
                value={value}
                onChange={onChange}
                onBlur={onBlur}
                name={name}
                placeholder={placeholder}
                className={className}
                disabled={disabled}
            />

            {status !== 'none' && (
                <span
                    style={{
                        position: 'absolute',
                        right: '20px',
                        top: '42%',
                        transform: 'translateY(-50%)'
                    }}
                >
                    {status === 'available' ? (
                        <Icon name="success" marginTop={2} width={successIconSize} height={successIconSize} />
                    ) : (
                        <Icon name="error" marginTop={2} width={errorIconSize} height={errorIconSize} />
                    )}
                </span>
            )}
        </div>
    );
}
